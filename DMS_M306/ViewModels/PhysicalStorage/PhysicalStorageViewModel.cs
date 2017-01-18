using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.ViewModels.PhysicalStorage
{
    public class PhysicalStorageViewModel
    {
        public int Id { get; set; }

        public string BuildingId { get; set; }

        public string RoomId { get; set; }

        public string CabinetId { get; set; }
    }
}