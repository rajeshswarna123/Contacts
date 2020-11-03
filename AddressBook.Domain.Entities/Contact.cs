using AddressBook.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook.Core.Entities
{
    public class Contact : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
    }
}
