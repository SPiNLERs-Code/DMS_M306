using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.ViewModels.File
{
    public class ReleaseViewModel
    {
        public int Id { get; set; }

        public int ReleaseNumber { get; set; }

        public string LastModifiedBy { get; set; }

        public string ReleasedBy { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Description { get; set; }
    }
}