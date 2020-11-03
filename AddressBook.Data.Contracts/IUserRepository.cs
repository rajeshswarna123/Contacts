using AddressBook.Core.Entities;
using System;

namespace AddressBook.Data.Contracts
{
    public interface IUserRepository
    {
        bool CreateUser(User user);

        User GetUserByEmail(string email);

        User GetUserById(string id);
    }
}
