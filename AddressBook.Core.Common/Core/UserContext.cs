using AddressBook.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBook.Core.Common.Core
{
    public class UserContext : IUserContext
    {
        public User UserInfo { get; set; }

        public string SessionId { get; set; }

        public DateTime? TokenExpiry { get; set; }

        public string TenantId { get; set; }
    }

    public interface IUserContext
    {
        User UserInfo { get; set; }

        string SessionId { get; set; }

        DateTime? TokenExpiry { get; set; }

        string TenantId { get; set; }

    }
}
