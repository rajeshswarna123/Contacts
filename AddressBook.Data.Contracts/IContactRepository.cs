using AddressBook.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook.Data.Contracts
{
    public interface IContactRepository
    {
        Contact CreateContact(Contact contact);

        Contact GetContactById(string id);

        List<Contact> GetAllContacts();

        List<Contact> GetActiveContacts();

        List<Contact> GetInActiveContacts();

        bool UpdateContact(Contact contact);

        bool DeleteContact(string id);

        bool DeleteContactPermenantly(string id);
    }
}
