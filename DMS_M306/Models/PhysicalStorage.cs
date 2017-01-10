using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.Models
{
    public class PhysicalStorage
    {
        public int Id { get; set; }

        public string BuildingId { get; set; }

        public string RoomId { get; set; }

        public string CabinetId { get; set; }

        public List<File> Files { get; set; }
    }
}