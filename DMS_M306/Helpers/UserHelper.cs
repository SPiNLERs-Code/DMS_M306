using DMS_M306.Interfaces.Repositories;
using DMS_M306.Models;
using System.Linq;

namespace DMS_M306.Helpers
{
    public static class UserHelper
    {
        public static User GetUserFromUserName(string userName, IUserRepository userRepository)
        {
            var user = userRepository.Get(x => x.UserName == userName).FirstOrDefault();
            return user;
        }
    }
}