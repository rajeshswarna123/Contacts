using AddressBook.Core.Models;
using AddressBook.Core.Models.Request;
using System.Collections.Generic;

namespace AddressBook.Domain.Contracts
{
    public interface IContactService
    {
        Contact CreateContact(ContactRequest contact);

        Contact GetContactById(string id);

        List<Contact> GetAllContacts();

        List<Contact> GetActiveContacts();

        List<Contact> GetInActiveContacts();

        bool UpdateContact(Contact contact);

        bool DeleteContact(string id);

        bool DeleteContactPermenantly(string id);
    }
}
