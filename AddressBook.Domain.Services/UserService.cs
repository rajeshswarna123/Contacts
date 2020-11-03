using AddressBook.Core.Common.Core;
using AddressBook.Core.Common.EncryptionHelper;
using AddressBook.Core.Common.JwtHelper;
using AddressBook.Core.Models;
using AddressBook.Core.Models.Request;
using AddressBook.Data.Contracts;
using AddressBook.Domain.Contracts;
using AutoMapper;
using System;
using System.Text;

namespace AddressBook.Domain.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;

        private readonly JwtIssuerOptions _jwtIssuerOptions;

        public UserService(JwtIssuerOptions jwtIssuerOptions,
                               IUserRepository userRepository,
                               IMapper mapper)
        {
            _jwtIssuerOptions = jwtIssuerOptions;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public bool Register(RegisterRequest registerRequest)
        {
            var user = _mapper.Map<Core.Entities.User>(registerRequest);
            user.Password = EncryptionHelper.GetMd5Hash(user.Password);
            return this._userRepository.CreateUser(user);
        }

        public User Login(LoginRequest request)
        {
            Core.Entities.User user = this.VerifyPassword(request.Email, request.Password);
            User userProfile = _mapper.Map<User>(user);
            userProfile.Token = JwtTokenHelper.GenerateJSONWebToken(this._jwtIssuerOptions, userProfile.Id, userProfile.Email);
            return userProfile;
        }

        public User GetUserById(string userId)
        {
            return _mapper.Map<User>(this._userRepository.GetUserById(userId));
        }

        private Core.Entities.User VerifyPassword(string email, string password)
        {
            Core.Entities.User user = this._userRepository.GetUserByEmail(email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (!EncryptionHelper.VerifyMd5Hash(password, user.Password))
            {
                throw new Exception("Invalid Credentials");
            }
            return user;
        }
    }
}
