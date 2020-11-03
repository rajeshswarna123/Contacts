using AddressBook.API.Filters;
using AddressBook.Core.Models;
using AddressBook.Core.Models.Request;
using AddressBook.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AddressBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [Authorize]
        [ServiceFilter(typeof(EnsureUserLoggedIn))]
        [Route("createcontact")]
        [HttpPost]
        public Contact CreateContact(ContactRequest contactRequest)
        {
            return this._contactService.CreateContact(contactRequest);
        }

        [Authorize]
        [ServiceFilter(typeof(EnsureUserLoggedIn))]
        [Route("getallcontacts")]
        [HttpGet]
        public List<Contact> GetAllContacts()
        {
            return this._contactService.GetAllContacts();
        }

        [Authorize]
        [ServiceFilter(typeof(EnsureUserLoggedIn))]
        [Route("getactivecontacts")]
        [HttpGet]
        public List<Contact> GetActiveContacts()
        {
            return this._contactService.GetActiveContacts();
        }

        [Authorize]
        [ServiceFilter(typeof(EnsureUserLoggedIn))]
        [Route("getinactivecontacts")]
        [HttpGet]
        public List<Contact> GetInActiveContacts()
        {
            return this._contactService.GetInActiveContacts();
        }

        [Authorize]
        [ServiceFilter(typeof(EnsureUserLoggedIn))]
        [Route("updatecontact")]
        [HttpPut]
        public bool UpdateContact(Contact contact)
        {
            return this._contactService.UpdateContact(contact);
        }

        [Authorize]
        [ServiceFilter(typeof(EnsureUserLoggedIn))]
        [Route("deletecontact")]
        [HttpPut]
        public bool DeleteContact(string id)
        {
            return this._contactService.DeleteContact(id);
        }

        [Authorize]
        [ServiceFilter(typeof(EnsureUserLoggedIn))]
        [Route("deletecontactpermanantly")]
        [HttpDelete]
        public bool DeleteContactPermanantly(string id)
        {
            return this._contactService.DeleteContactPermenantly(id);
        }
    }
}
