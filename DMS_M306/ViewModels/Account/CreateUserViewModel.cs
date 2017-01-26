using DMS_M306.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.ViewModels.Account
{
    public class CreateUserViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public UserRoles Role{ get; set; }
    }
}