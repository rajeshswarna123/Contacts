using AddressBook.Core.Common;
using AddressBook.Core.Common.Core;
using AddressBook.Core.Common.SerializeHelper;
using AddressBook.Core.Entities.Base;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Authentication;

namespace AddressBook.Data.Respositories.Base
{
    public class TypedBaseRepository<T> 
       where T : IEntity
    {
        protected readonly IMongoCollection<T> _myCollection;

        protected string _shardKey;

        private readonly IUserContext _userContext;

        public IMongoApiDatabaseSettings _settings { get; set; }



        public TypedBaseRepository(IOptions<MongoApiDatabaseSettings> settings, string collectionName, string shardKey, IUserContext userContext)
        {
            _settings = settings.Value;
            // register the conventions
            var pack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
            pack.AddMemberMapConvention(
                "LowerCaseElementName",
                m => m.SetElementName(m.MemberName.ToLower()));
            ConventionRegistry.Register("My Custom Conventions", pack, t => true);

            // setup connection
            MongoClientSettings msettings = MongoClientSettings.FromUrl(
  new MongoUrl(this._settings.ConnectionString));
            msettings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(msettings);

            var database = mongoClient.GetDatabase(this._settings.DatabaseName);

            this._shardKey = shardKey;

            this._myCollection = database.GetCollection<T>(collectionName);

            this._userContext = userContext;
        }

        public List<T> Get() =>
            this._myCollection.Find(item => true).ToList();

        public T FindOne(Expression<Func<T, string>> filterExpression, string matchingValue)
        {
            var builder = Builders<T>.Filter;
            var filter = builder.Eq(filterExpression, matchingValue);
            return this._myCollection.Find<T>(filter).FirstOrDefault();
        }

        public T Create(T item)
        {
            if (this._userContext.UserInfo != null)
            {
                item.CreatedBy = this._userContext.UserInfo.Email;
                item.UpdatedBy = this._userContext.UserInfo.Email;

            }
            item.CreatedDate = DateTime.Now;
            item.UpdatedDate = DateTime.Now;
            item.shardkey = this._shardKey;
            this._myCollection.InsertOne(item);
            return item;
        }

        public bool Update(string id, UpdateDefinition<T> updateDefinition)
        {
            if((this._userContext!=null) && (this._userContext.UserInfo !=null))
                updateDefinition=updateDefinition.Set(_ => _.UpdatedBy, this._userContext.UserInfo.Email).
                Set(_ => _.UpdatedDate, DateTime.Now);
            var modifications = this._myCollection.UpdateOne(item => item.shardkey == this._shardKey && item.Id == id, updateDefinition);
            return modifications.ModifiedCount > 0;
        }

        public bool Remove(string id) =>
            this._myCollection.DeleteOne(item => item.shardkey == this._shardKey && item.Id == id).DeletedCount > 0;


    }
}
