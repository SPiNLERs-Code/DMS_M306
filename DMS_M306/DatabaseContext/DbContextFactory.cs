using DMS_M306.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DMS_M306.DatabaseContext
{
    public class DbContextFactory : IDbContextFactory<DMSEntities>
    {
        private readonly IDatabaseInitializer<DMSEntities> dbInitializer;

        private bool hasSetInitializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextFactory"/> class.
        /// </summary>
        /// <param name="dbInitializer">The database initializer.</param>
        public DbContextFactory(IDatabaseInitializer<DMSEntities> dbInitializer)
        {
            this.dbInitializer = dbInitializer;
        }


        /// <summary>
        /// Build the Entity Framework database
        /// context based on our entities.
        /// </summary>
        /// <returns>The new database context.</returns>
        public DMSEntities Build()
        {
            if (!this.hasSetInitializer)
            {
                Database.SetInitializer(this.dbInitializer);
                this.hasSetInitializer = true;
            }

            var context = new DMSEntities();
            return context;
        }

    }
}