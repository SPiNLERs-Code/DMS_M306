using DMS_M306.Interfaces;
using DMS_M306.Interfaces.Repositories;
using DMS_M306.Models;

namespace DMS_M306.Repositories
{
    public class FileRepository : RepositoryBase<File>, IFileRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileRepository"/> class.
        /// </summary>
        /// <param name="contextManager">The context manager.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public FileRepository(IDbContextManager contextManager)
            : base(contextManager)
        {
        }
    }
}