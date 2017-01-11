using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DMS_M306.DatabaseContext
{
    public class CreateDatabaseIfNotExist : CreateDatabaseIfNotExists<DMSEntities>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropCreateDatabaseIfModelChangesInitializer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        public CreateDatabaseIfNotExist()
        {

        }
    }
}