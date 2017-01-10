using DMS_M306.Interfaces;
using DMS_M306.Interfaces.Repositories;
using DMS_M306.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMS_M306.Repositories
{
    public class ChangeRepository : RepositoryBase<Change>, IChangeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeRepository"/> class.
        /// </summary>
        /// <param name="contextManager">The context manager.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public ChangeRepository(IDbContextManager contextManager)
            : base(contextManager)
        {
        }
    }
}