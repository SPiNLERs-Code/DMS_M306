using DMS_M306.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.Models
{
    public class File
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string StorageName { get; set; }

        public string FileEnding { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastModified { get; set; }

        public virtual PhysicalStorage PhysicalStorage { get; set; }

        public int? PhysicalStorageId { get; set; }

        public virtual User CreatedBy { get; set; }

        public int CreatedById { get; set; }

        public virtual User LastModifiedBy { get; set; }

        public int LastModifiedById { get; set; }

        public FileStorageType StorageType { get; set; }

        public FileClass Class { get; set; }

        public FileStatus Status { get; set; }

        public virtual List<Change> Changes { get; set; }

        public virtual List<Release> Releases { get; set; }

        public virtual FileCategory Category { get; set; }

        public int CategoryId { get; set; }

    }
}