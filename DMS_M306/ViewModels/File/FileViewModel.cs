using DMS_M306.Enums;
using DMS_M306.ViewModels.Change;
using DMS_M306.ViewModels.FileCategory;
using DMS_M306.ViewModels.PhysicalStorage;
using DMS_M306.ViewModels.FileRelease;
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
        private DateTime _createDate;
        private DateTime _lastModified;

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate
        {
            get
            {
                return _createDate.ToLocalTime();
            }
            set
            {
                _createDate = value;
            }
        }

        public DateTime LastModified
        {
            get
            {
                return _lastModified.ToLocalTime();
            }
            set
            {
                _lastModified = value;
            }
        }

        public string CreatedBy { get; set; }

        public string LastModifiedBy { get; set; }

        public FileStorageType StorageType { get; set; }

        public FileClass Class { get; set; }

        public FileStatus Status { get; set; }

        public int ReleaseCount { get; set; }

        public string Category { get; set; }

        public PhysicalStorageViewModel PhysicalStorage { get; set; }
    }
}