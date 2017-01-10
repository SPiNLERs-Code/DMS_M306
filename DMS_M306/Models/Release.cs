using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.Models
{
    public class Release
    {
        public int Id { get; set; }

        public File RootFile { get; set; }

        public int RootFileId { get; set; }

        public User LastModifiedBy { get; set; }

        public int LastModifiedById { get; set; }

        public User ReleasedBy { get; set; }

        public int ReleasedById { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Description { get; set; }
    }
}