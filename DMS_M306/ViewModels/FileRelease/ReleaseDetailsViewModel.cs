using DMS_M306.ViewModels.PhysicalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.ViewModels.FileRelease
{
    public class ReleaseDetailsViewModel
    {
        private DateTime _releaseDate;
        public int Id { get; set; }

        public int ReleaseNumber { get; set; }

        public string RootFileName { get; set; }

        public int RootFileId { get; set; }

        public PhysicalStorageViewModel PhysicalStorage { get; set; }

        public string QRString
        {
            get
            {
                string rootFileString = RootFileId.ToString("X6");
                string idString = ReleaseNumber.ToString("X4");
                string fullString = rootFileString + "-" + idString;
                return fullString;
            }
        }

        public string Description { get; set; }

        public string LastModifiedBy { get; set; }

        public string ReleasedBy { get; set; }

        public DateTime ReleaseDate
        {
            get
            {
                return _releaseDate.ToLocalTime();
            }
            set
            {
                _releaseDate = value;
            }
        }

        public bool IsActive { get; set; }
    }
}