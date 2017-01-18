using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace DMS_M306.Models.Mapping
{
    public class ReleaseMap : EntityTypeConfiguration<Release>
    {
        public ReleaseMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Table & Column Mappings
            this.ToTable("Release");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.ReleaseDate).HasColumnName("ReleaseDate");
            this.Property(t => t.ReleaseNumber).HasColumnName("ReleaseNumber");

            // Relationships
            this.HasRequired(x => x.LastModifiedBy)
                .WithMany(x => x.LastModiefiedByReleases).HasForeignKey(x => x.LastModifiedById).WillCascadeOnDelete(false);
            this.HasRequired(x => x.ReleasedBy)
                .WithMany(x => x.Releases).HasForeignKey(x => x.ReleasedById).WillCascadeOnDelete(false);
            this.HasRequired(x => x.RootFile)
                .WithMany(x => x.Releases).HasForeignKey(x => x.RootFileId).WillCascadeOnDelete(false);
        }

    }
}