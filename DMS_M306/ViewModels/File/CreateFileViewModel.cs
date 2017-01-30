using DMS_M306.Attributes;
using DMS_M306.Enums;
using DMS_M306.ViewModels.PhysicalStorage;
using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS_M306.ViewModels.File
{
    public class CreateFileViewModel
    {
        public int? Id { get; set; }

        [Required]
        [DisplayName("Name:")]
        [RegularExpression(@"^[\w\-. ]+$",
         ErrorMessage = "Only valide Filenames are allowed.")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Description:")]
        public string Description { get; set; }

        public PhysicalStorageViewModel PhysicalStorage { get; set; }

        [Required]
        [MinValueValidationAttribute(1, ErrorMessage = "Das Feld \"StorageType\" ist erforderlich.")]
        [DisplayName("Storage type:")]
        public FileStorageType StorageType { get; set; }

        [Required]
        [MinValueValidationAttribute(1, ErrorMessage = "Das Feld \"Class\" ist erforderlich.")]
        [DisplayName("Classification:")]
        public FileClass Class { get; set; }

        [Required]
        [MinValueValidationAttribute(1,ErrorMessage = "Das Feld \"Status\" ist erforderlich.")]
        [DisplayName("Status:")]
        public FileStatus Status { get; set; }

        public List<SelectListItem> Categories { get; set; }

        [Required]
        [DisplayName("Category:")]
        public int CategoryId { get; set; }

        public string FormInformation { get; set; }

        [RequiredIfTrue("IsFilePhysical", ErrorMessage = "")]
        [DisplayName("Building ID/Name:")]
        public string PhysicalStorageBuildingId { get; set; }

        [RequiredIfTrue("IsFilePhysical", ErrorMessage = "")]
        [DisplayName("Cabinet ID/Name:")]
        public string PhysicalStorageCabinetId { get; set; }

        [RequiredIfTrue("IsFilePhysical",ErrorMessage ="")]
        [DisplayName("Room ID/Name:")]
        public string PhysicalStorageRoomId { get; set; }

        public bool IsFilePhysical
        {
            get
            {
                if (StorageType == FileStorageType.PhysicalStorage) return true;
                return false;
            }
        }
    }
}