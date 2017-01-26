using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.ViewModels.Change
{
    public class ChangeViewModel
    {
        private DateTime _changeDate;

        public int FileId { get; set; }

        public string Description { get; set; }

        public DateTime ChangeDate
        {
            get
            {
                return _changeDate.ToLocalTime();
            }
            set
            {
                _changeDate = value;
            }
        }

        public string ChangedBy { get; set; }

    }
}