using System;

namespace DMS_M306.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Commits the changes.
        /// </summary>
        void SaveChanges();

        #endregion
    }
}