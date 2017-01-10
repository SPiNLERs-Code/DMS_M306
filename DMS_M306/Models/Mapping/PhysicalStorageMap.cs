using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace DMS_M306.Models.Mapping
{
    public class PhysicalStorageMap : EntityTypeConfiguration<PhysicalStorage>
    {
        public PhysicalStorageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Table & Column Mappings
            this.ToTable("PhysicalStorage");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BuildingId).HasColumnName("BuildingId");
            this.Property(t => t.CabinetId).HasColumnName("CabinetId");
            this.Property(t => t.RoomId).HasColumnName("RoomId");
        }

    }
}