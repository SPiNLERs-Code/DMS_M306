

using System.ComponentModel.DataAnnotations;

namespace DMS_M306.ViewModels.FileRelease
{
    public class EditReleaseViewModel
    {
        [Required]
        public int Id { get; set; }

        public string RootFileName { get; set; }

        public string ReleaseNumber { get; set; }

        public bool IsActive { get; set; }
    }
}