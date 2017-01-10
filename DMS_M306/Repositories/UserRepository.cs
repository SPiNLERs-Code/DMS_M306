using DMS_M306.Interfaces;
using DMS_M306.Interfaces.Repositories;
using DMS_M306.Models;

namespace DMS_M306.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="contextManager">The context manager.</param>
        public UserRepository(IDbContextManager contextManager)
            : base(contextManager)
        {
        }
    }
}