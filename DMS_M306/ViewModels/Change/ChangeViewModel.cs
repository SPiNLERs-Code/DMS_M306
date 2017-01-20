using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.ViewModels.Change
{
    public class ChangeViewModel
    {
        public int FileId { get; set; }

        public string Description { get; set; }

        public DateTime ChangeDate { get; set; }

        public string ChangedBy { get; set; }

    }
}