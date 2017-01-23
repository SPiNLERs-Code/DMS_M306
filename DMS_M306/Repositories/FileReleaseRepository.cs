using DMS_M306.Interfaces;
using DMS_M306.Interfaces.Repositories;
using DMS_M306.Models;

namespace DMS_M306.Repositories
{
    public class FileReleaseRepository : RepositoryBase<FileRelease>, IFileReleaseRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileRepository"/> class.
        /// </summary>
        /// <param name="contextManager">The context manager.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public FileReleaseRepository(IDbContextManager contextManager)
            : base(contextManager)
        {
        }
    }
}