using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook.Core.Models.Request
{
    public class ContactRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
    }
}
