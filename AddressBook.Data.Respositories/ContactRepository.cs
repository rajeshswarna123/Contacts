using AddressBook.Core.Common.Core;
using AddressBook.Core.Entities;
using AddressBook.Data.Contracts;
using AddressBook.Data.Respositories.Base;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook.Data.Respositories
{
    public class ContactRepository:TypedBaseRepository<Contact>, IContactRepository
    {
        protected readonly IMongoCollection<Contact> _contacts;

        private readonly static string _collectionName = "contacts";

        public ContactRepository(IOptions<MongoApiDatabaseSettings> settings, IUserContext userContext)
            : base(settings, _collectionName, settings.Value.ContactsCollectionSharedKey, userContext)
        {
            this._contacts = this._myCollection;
        }

        public Contact CreateContact(Contact contact)
        {
            return this.Create(contact);
        }

        public Contact GetContactById(string id)
        {
            return this.FindOne(user => user.Id, id);
        }

        public List<Contact> GetAllContacts()
        {
            return this.Get();
        }

        public List<Contact> GetActiveContacts()
        {
            return this._contacts.Find(user => user.Active==true).ToList();
        }

        public List<Contact> GetInActiveContacts()
        {
            return this._contacts.Find(user => user.Active == false).ToList();
        }

        public bool UpdateContact(Contact contact)
        {
            var updateDefinition = Builders<Contact>.Update.
                Set(_ => _.FirstName, contact.FirstName).
                Set(_ => _.LastName, contact.LastName).
                Set(_ => _.PhoneNumber, contact.PhoneNumber);

            return this.Update(contact.Id, updateDefinition);
        }

        public bool DeleteContact(string id)
        {
            var updateDefinition = Builders<Contact>.Update.
               Set(_ => _.Active, false);

            return this.Update(id, updateDefinition);
        }

        public bool DeleteContactPermenantly(string id)
        {
            return this.Remove(id);
        }
    }
}
