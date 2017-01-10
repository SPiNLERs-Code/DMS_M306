using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.Models
{
    public class Change
    {
        public int Id { get; set; }

        public File ChangedFile { get; set; }

        public User ChangedBy { get; set; }

        public DateTime ChangeDate { get; set; }

        public string Description { get; set; }
    }
}