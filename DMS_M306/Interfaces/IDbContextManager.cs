using System.Data.Entity;

namespace DMS_M306.Interfaces
{
    #region using statements

    using System;
    using System.Data.Entity;

    #endregion

    /// <summary>
    /// The database context manager interface.
    /// </summary>
    public interface IDbContextManager : IDisposable
    {
        #region Public Properties

        /// <summary>
        /// Gets the context.
        /// </summary>
        DbContext Context { get; }

        /// <summary>
        /// Gets a value indicating whether has context.
        /// </summary>
        bool HasContext { get; }

        #endregion
    }

    /// <summary>
    /// The database context manager interface.
    /// </summary>
    /// <typeparam name="T">Type to be managed.</typeparam>
    public interface IDbContextManager<T> : IDbContextManager
    {
    }
}