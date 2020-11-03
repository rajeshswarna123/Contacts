using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook.Core.Models.Request
{
    public class LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
