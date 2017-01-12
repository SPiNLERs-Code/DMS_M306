using DMS_M306.Enums;
using DMS_M306.ViewModels.Change;
using DMS_M306.ViewModels.FileCategory;
using DMS_M306.ViewModels.PhysicalStorage;
using DMS_M306.ViewModels.Release;
using DMS_M306.ViewModels.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS_M306.ViewModels.File
{
    public class FileViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string StorageName { get; set; }
        
        [Required]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastModified { get; set; }

        public PhysicalStorageViewModel PhysicalStorage { get; set; }

        public int PhysicalStorageId { get; set; }

        public UserViewModel CreatedBy { get; set; }

        public int CreatedById { get; set; }

        public UserViewModel LastModifiedBy { get; set; }

        public int LastModifiedById { get; set; }

        public FileStorageType StorageType { get; set; }

        public FileClass Class { get; set; }

        public FileStatus Status { get; set; }

        public List<ChangeViewModel> Changes { get; set; }

        public List<ReleaseViewModel> Releases { get; set; }

        public FileCategoryViewModel Category { get; set; }

        public int CategoryId { get; set; }
    }
}