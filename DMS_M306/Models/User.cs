using DMS_M306.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string Email { get; set; }

        public UserRoles Role { get; set; }

        public List<Change> Changes { get; set; }

        public List<File> CreatedFiles { get; set; }

        public List<Release> Releases { get; set; }

        public List<Release> LastModiefiedByReleases { get; set; }

        public List<File> LastModifiedFiles { get; set; }

    }
}