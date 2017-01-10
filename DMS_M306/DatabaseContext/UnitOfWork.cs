using DMS_M306.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace DMS_M306.DatabaseContext
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextManager contextManager;

        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork" /> class.
        /// </summary>
        /// <param name="contextManager">The context manager.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public UnitOfWork(IDbContextManager contextManager)
        {

            this.contextManager = contextManager;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        ~UnitOfWork()
        {
            this.Dispose(false);
        }

        #region IUnitOfWork Members

        /// <summary>
        /// Save any change made to the
        /// database as long as there is
        /// a context to manage..
        /// </summary>
        public void SaveChanges()
        {
            // TODO: is null somethimes?
            if (this.contextManager.HasContext)
            {
                this.contextManager.Context.SaveChanges();
            }
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
                    if (this.contextManager != null)
                    {
                        this.contextManager.Dispose();

                        this.contextManager = null;
                    }
                }

                this.disposed = true;
            }
        }

        #endregion
    }
}