using DMS_M306.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace DMS_M306.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

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

        public virtual UserRoles Role { get; set; }

        public virtual List<Change> Changes { get; set; }

        public virtual List<File> CreatedFiles { get; set; }

        public virtual List<FileRelease> Releases { get; set; }

        public virtual List<FileRelease> LastModiefiedByReleases { get; set; }

        public virtual List<File> LastModifiedFiles { get; set; }
    }
}