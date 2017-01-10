using DMS_M306.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DMS_M306.DatabaseContext
{
    public class DbContextManager<T> : IDbContextManager<T>
        where T : DbContext
    {
        private readonly IDbContextFactory<T> factory;

        private T context;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextManager{T}"/> class.
        /// </summary>
        /// <param name="factory">The factory that creates a context.</param>
        public DbContextManager(IDbContextFactory<T> factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DbContextManager{T}"/> class.
        /// </summary>
        ~DbContextManager()
        {
            this.Dispose(false);
        }

        #region IDbContextManager<T> Members

        /// <summary>
        /// Gets the context,
        /// creating it if doesn't
        /// exist.
        /// </summary>
        public DbContext Context
        {
            get
            {
                return this.context ?? (this.context = this.factory.Build());
            }
        }

        /// <summary>
        /// Gets a value indicating whether context
        /// already exists.
        /// </summary>
        public bool HasContext
        {
            get { return this.Context != null; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed == false)
            {
                if (disposing)
                {
                    if (this.HasContext)
                    {
                        this.context.Dispose();
                        this.context = null;
                    }
                }

                this.disposed = true;
            }
        }

        #endregion
    }
}