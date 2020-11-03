using AddressBook.Core.Models;
using AddressBook.Core.Models.Request;

namespace AddressBook.Domain.Contracts
{
    public interface IUserService
    {
        bool Register(RegisterRequest registerRequest);
        User Login(LoginRequest loginRequest);
        User GetUserById(string userId);
    }
}
