using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DMS_M306.Interfaces
{
    public interface IUserManager
    {
        ClaimsIdentity GetIdentityByLoginCredentials(string username, string password);
    }
}
