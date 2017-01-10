// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the basic functionality of a repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DMS_M306.Interfaces.Repositories
{
    #region using statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    #endregion

    /// <summary>
    /// Defines the basic functionality of a repository.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of the entity.
    /// </typeparam>
    public interface IRepository<TEntity>
        where TEntity : class
    {
        // RETRIEVE METHODS
        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        void Add(TEntity entity);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        void Delete(object id);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="entityToDelete">
        /// The entity to delete.
        /// </param>
        void Delete(TEntity entityToDelete);

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="TEntity"/>.
        /// </returns>
        TEntity Find(object id);

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<TEntity> Get();

        /// <summary>
        /// The load collection.
        /// </summary>
        /// <typeparam name="TElement">The type of collection elements.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="expression">The expression.</param>
        void LoadCollection<TElement>(TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> expression)
            where TElement : class;

        /// <summary>
        /// The load reference.
        /// </summary>
        /// <typeparam name="TProperty">The property to be loaded.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="expression">The expression.</param>
        void LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> expression)
            where TProperty : class;

        // MODIFICATION METHODS

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entityToUpdate">
        /// The entity to update.
        /// </param>
        void Update(TEntity entityToUpdate);

        #endregion

        //// SAVE is not implelented in the repository
        //// because we might want to commit changes to the database from 
        //// multiple repositories, save is called from the EfUnitOfWork
    }
}