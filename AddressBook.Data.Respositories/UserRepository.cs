using AddressBook.Core.Common.Core;
using AddressBook.Core.Entities;
using AddressBook.Data.Contracts;
using AddressBook.Data.Respositories.Base;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AddressBook.Data.Respositories
{
    public class UserRepository : TypedBaseRepository<User>, IUserRepository
    {
        protected readonly IMongoCollection<User> _users;

        private readonly static string _collectionName = "users";

        public UserRepository(IOptions<MongoApiDatabaseSettings> settings, IUserContext userContext)
            : base(settings, _collectionName, settings.Value.UsersCollectionShardKey, userContext)
        {
            this._users = this._myCollection;
        }

        public bool CreateUser(User user)
        {
            var createdUser = this.Create(user);
            return createdUser.Id != null ? true : false;
        }

        public User GetUserById(string id)
        {
            return this.FindOne(user => user.Id, id);
        }

        public User GetUserByEmail(string email)
        {
            return this.FindOne(user => user.Email, email.ToLower());
        }
    }
}
