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

namespace DMS_M306.ViewModels.File
{
    public class EditFileViewModel
    {
        public int Id { get; set; }

        [DisplayName("Name:")]
        public string Name { get; set; }

        public int FileId { get; set; }

        [Required]
        [DisplayName("Description:")]
        public string Description { get; set; }

        public PhysicalStorageViewModel PhysicalStorage { get; set; }

        public FileStorageType StorageType { get; set; }


        [DisplayName("StorageType:")]
        public string StorageTypeName { get
            {
                return StorageType.ToString();
            }
        }
        [DisplayName("FileType:")]
        public string FileType { get; set; }

        [Required]
        [MinValueValidationAttribute(1, ErrorMessage = "Das Feld \"Class\" ist erforderlich.")]
        [DisplayName("Classification:")]
        public FileClass Class { get; set; }

        [Required]
        [MinValueValidationAttribute(1, ErrorMessage = "Das Feld \"Status\" ist erforderlich.")]
        [DisplayName("Status:")]
        public FileStatus Status { get; set; }

        
        [DisplayName("Category:")]
        public string CategoryName { get; set; }

        public string FormInformation { get; set; }

        [RequiredIfTrue("IsFilePhysical", ErrorMessage = "")]
        [DisplayName("Building ID/Name:")]
        public string PhysicalStorageBuildingId { get; set; }

        [RequiredIfTrue("IsFilePhysical", ErrorMessage = "")]
        [DisplayName("Cabinet ID/Name:")]
        public string PhysicalStorageCabinetId { get; set; }

        [RequiredIfTrue("IsFilePhysical", ErrorMessage = "")]
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