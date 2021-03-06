﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace DMS_M306.Models.Mapping
{
    public class FileMap : EntityTypeConfiguration<File>
    {
        public FileMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Table & Column Mappings
            this.ToTable("File");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Class).HasColumnName("Class");
            this.Property(t => t.StorageName).HasColumnName("StorageName");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.LastModified).HasColumnName("LastModified");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.StorageType).HasColumnName("StorageType");
            this.Property(t => t.FileEnding).HasColumnName("FileEnding");

            // Relationships
            this.HasRequired(x => x.CreatedBy)
                .WithMany(x => x.CreatedFiles).HasForeignKey(x => x.CreatedById).WillCascadeOnDelete(false);
            this.HasRequired(x => x.LastModifiedBy)
                .WithMany(x => x.LastModifiedFiles).HasForeignKey(x => x.LastModifiedById).WillCascadeOnDelete(false);
            this.HasRequired(x => x.Category)
                .WithMany(x => x.Files).HasForeignKey(x => x.CategoryId).WillCascadeOnDelete(false);

            this.HasOptional(x => x.PhysicalStorage)
               .WithMany(x => x.Files).HasForeignKey(x => x.PhysicalStorageId).WillCascadeOnDelete(false);
        }
    
    }
}