using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DMS_M306.ViewModels.Release
{
    public class CreateForViewModel
    {
        [Required]
        public int FileId { get; set; }

        public string FileName { get; set; }

        [Required]
        public string Description { get; set; }
    }
}