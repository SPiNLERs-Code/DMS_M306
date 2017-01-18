using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.Models
{
    public class Change
    {
        public int Id { get; set; }

        public virtual File ChangedFile { get; set; }

        public int ChangedFileId { get; set; }

        public virtual User ChangedBy { get; set; }

        public int ChangedById { get; set; }

        public DateTime ChangeDate { get; set; }

        public string Description { get; set; }
    }
}