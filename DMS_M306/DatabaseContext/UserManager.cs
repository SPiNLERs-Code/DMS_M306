using DMS_M306.Interfaces;
using DMS_M306.Interfaces.Repositories;
using DMS_M306.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace DMS_M306.DatabaseContext
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ClaimsIdentity GetIdentityByLoginCredentials(string username, string password)
        {
            var user = GetUserByCredentials(username, password);
            if (user != null)
            {
                return new ClaimsIdentity(
                  new[] { 
              // adding following 2 claim just for supporting default antiforgery provider
              new Claim(ClaimTypes.NameIdentifier, username),
              new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

              new Claim(ClaimTypes.Name,username),

              // optionally you could add roles if any
              new Claim(ClaimTypes.Role, user.Role.ToString()),

                  },
                  DefaultAuthenticationTypes.ApplicationCookie);
            }
            return null;
        }

        private User GetUserByCredentials(string username, string password)
        {
            return _userRepository.Get(x => x.UserName == username && x.Password == password).FirstOrDefault();
        }
    }
}