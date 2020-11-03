using AddressBook.Core.Models.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook.Core.Models
{
    public class Contact : ContactRequest
    {
        public string Id { get; set; }

    }
}
