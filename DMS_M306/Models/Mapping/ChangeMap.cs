using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace DMS_M306.Models.Mapping
{
    public class ChangeMap : EntityTypeConfiguration<Change>
    {
        public ChangeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("Change");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ChangeDate).HasColumnName("ChangeDate");
            this.Property(t => t.Description).HasColumnName("Description");

            // Relationships
            this.HasRequired(x => x.ChangedBy)
                .WithMany(x => x.Changes).HasForeignKey(x => x.ChangedById).WillCascadeOnDelete(false);
            this.HasRequired(x => x.ChangedFile)
                .WithMany(x => x.Changes).HasForeignKey(x => x.ChangedFileId).WillCascadeOnDelete(false);
        }
    }
}