using AddressBook.Core.Models;
using AddressBook.Core.Models.Request;
using AddressBook.Data.Contracts;
using AddressBook.Domain.Contracts;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace AddressBook.Domain.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        private readonly IMapper _mapper;

        public ContactService(IContactRepository contactRepository,
                                IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }
        public Contact CreateContact(ContactRequest contactRequest)
        {
            var contact = this._contactRepository.CreateContact(_mapper.Map<Core.Entities.Contact>(contactRequest));
            return _mapper.Map<Contact>(contact);
        }

        public List<Contact> GetActiveContacts()
        {
            var contacts = this._contactRepository.GetActiveContacts();
            return _mapper.Map<List<Contact>>(contacts);
        }

        public List<Contact> GetAllContacts()
        {
            var contacts = this._contactRepository.GetAllContacts();
            return _mapper.Map<List<Contact>>(contacts);
        }

        public List<Contact> GetInActiveContacts()
        {
            var contacts = this._contactRepository.GetInActiveContacts();
            return _mapper.Map<List<Contact>>(contacts);
        }

        public Contact GetContactById(string id)
        {
            var contact = this._contactRepository.GetContactById(id);
            return _mapper.Map<Contact>(contact);
        }

        public bool UpdateContact(Contact contact)
        {
            return this._contactRepository.UpdateContact(_mapper.Map<Core.Entities.Contact>(contact));
        }

        public bool DeleteContact(string id)
        {
            return this._contactRepository.DeleteContact(id);
        }

        public bool DeleteContactPermenantly(string id)
        {
            return this._contactRepository.DeleteContactPermenantly(id);
        }
    }
}
