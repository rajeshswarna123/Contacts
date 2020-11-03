using AddressBook.Core.Models.Request;
using System;

namespace AddressBook.Core.Models
{
    public class User : LoginRequest
    {
        public string Id { get; set; }
        public string Token { get; set; }

    }
}
