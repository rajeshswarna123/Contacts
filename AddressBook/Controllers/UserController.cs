using AddressBook.Core.Models;
using AddressBook.Core.Models.Request;
using AddressBook.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.API.Controllers.Public
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [Route("register")]
        [HttpPost]
        public bool Register(RegisterRequest registerRequest)
        {
            return this._userService.Register(registerRequest);
        }

        [Route("login")]
        [HttpPost]
        public User Login(LoginRequest loginRequest)
        {
            return this._userService.Login(loginRequest);
        }
    }
}
