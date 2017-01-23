using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.Models
{
    public class FileRelease
    {
        public int Id { get; set; }

        public int ReleaseNumber { get; set; }

        public virtual File RootFile { get; set; }

        public int RootFileId { get; set; }

        public virtual User LastModifiedBy { get; set; }

        public int LastModifiedById { get; set; }

        public virtual User ReleasedBy { get; set; }

        public int ReleasedById { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Description { get; set; }
    }
}