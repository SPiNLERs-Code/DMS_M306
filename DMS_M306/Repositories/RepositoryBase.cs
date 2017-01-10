using DMS_M306.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Data.Entity;
using DMS_M306.Interfaces;

namespace DMS_M306.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
        /// </summary>
        /// <param name="contextManager">The context manager.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        protected RepositoryBase(IDbContextManager contextManager)
        {
            if (contextManager == null) throw new NullReferenceException();

            this.ContextManager = contextManager;
        }

        /// <summary>
        /// Gets the transaction context.
        /// </summary>
        /// <value>The context.</value>
        protected DbContext Context
        {
            get
            {
                return this.ContextManager.Context;
            }
        }

        /// <summary>
        /// Gets the EntityFramework context manager.
        /// </summary>
        /// <value>The context manager.</value>
        protected IDbContextManager ContextManager { get; private set; }

        private IDbSet<T> DbSet
        {
            get
            {
                return this.Context.Set<T>();
            }
        }

        #region IRepository<T> Members

        /// <summary>
        /// The get interface that allows filtering.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The <see cref="IQueryable" />Interface to be lazy loaded.</returns>
        public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter)
        {
            return this.Get().Where(filter);
        }

        /// <summary>
        /// The get interface that retrieves full set.
        /// </summary>
        /// <returns>The <see cref="IQueryable" />Interface to be lazy loaded.</returns>
        public virtual IQueryable<T> Get()
        {
            return this.DbSet;
        }

        /// <summary>
        /// The find interface invoked to
        /// find instance with parameter id.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>
        /// The <see cref="T" />Entity that we are searching for.</returns>
        public virtual T Find(object id)
        {
            return this.DbSet.Find(id);
        }

        /// <summary>
        /// The interface that allows adding
        /// and instance to this repository.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        public virtual void Add(T entity)
        {
            this.DbSet.Add(entity);
        }

        /// <summary>
        /// The delete interface that allows deleting
        /// of an instance with the parameter id.
        /// </summary>
        /// <param name="id">The id of instance to delete.</param>
        public virtual void Delete(object id)
        {
            var entityToDelete = this.DbSet.Find(id);

            this.Delete(entityToDelete);
        }

        /// <summary>
        /// The interface that allows deleting instance
        /// from this repository.
        /// </summary>
        /// <param name="entityToDelete">An entity of this repository to be deleted.</param>
        public virtual void Delete(T entityToDelete)
        {
            if (this.Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.DbSet.Attach(entityToDelete);
            }

            this.DbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// The interface that allows updating of an
        /// instance with the parameter instance.
        /// </summary>
        /// <param name="entityToUpdate">The entity that will update.</param>
        public virtual void Update(T entityToUpdate)
        {
            this.DbSet.Attach(entityToUpdate);
            this.Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        /// The interface implementation that allows loading of the
        /// entire set of the entity this instance manages.
        /// </summary>
        /// <typeparam name="TElement">The type of collection elements.</typeparam>
        /// <param name="entity">The entity to load.</param>
        /// <param name="expression">The selection expression.</param>
        public virtual void LoadCollection<TElement>(T entity, Expression<Func<T, ICollection<TElement>>> expression) where TElement : class
        {
            this.Context.Entry(entity).Collection(expression).Load();
        }

        public virtual void LoadReference<TProperty>(T entity, Expression<Func<T, TProperty>> expression) where TProperty : class
        {
            this.Context.Entry(entity).Reference(expression).Load();
        }

        #endregion
    }
}