﻿using DMS_M306.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DMS_M306.DatabaseContext
{
    public class DMSEntities : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DMSEntities"/> class.
        /// </summary>
        public DMSEntities()
            : base("Name=DMSEntities")
        {
        }


        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuilder, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ChangeMap());
            modelBuilder.Configurations.Add(new FileCategoryMap());
            modelBuilder.Configurations.Add(new FileMap());
            modelBuilder.Configurations.Add(new PhysicalStorageMap());
            modelBuilder.Configurations.Add(new FileReleaseMap());
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}