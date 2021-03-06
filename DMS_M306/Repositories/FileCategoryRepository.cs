﻿using DMS_M306.Interfaces;
using DMS_M306.Interfaces.Repositories;
using DMS_M306.Models;

namespace DMS_M306.Repositories
{
    public class FileCategoryRepository : RepositoryBase<FileCategory>, IFileCategoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeRepository"/> class.
        /// </summary>
        /// <param name="contextManager">The context manager.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public FileCategoryRepository(IDbContextManager contextManager)
            : base(contextManager)
        {
        }
    }
}