using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MistralMovieRating.Repository
{
    /// <summary>
    /// Generic repository for entity CRUD operations.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Returns all entities.
        /// </summary>
        /// <param name="include">A function to include navigation properties.</param>
        /// <param name="disableTracking"><c>true</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="ignoreQueryFilters">Ignore query filters.</param>
        /// <returns>List of TEntity.</returns>
        Task<List<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, bool ignoreQueryFilters = false);

        /// <summary>
        /// Returns entity by Id.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="include">A function to include navigation properties.</param>
        /// <param name="disableTracking"><c>true</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="ignoreQueryFilters">Ignore query filters.</param>
        /// <returns>TEntity.</returns>
        Task<TEntity> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null, bool disableTracking = true, bool ignoreQueryFilters = false);

        /// <summary>
        /// Inserts new entity to database.
        /// No effect unitl context.SaveChanges() is called.
        /// </summary>
        /// <param name="entity">TEntity.</param>
        /// <returns>New TEntity.</returns>
        Task<TEntity> Insert(TEntity entity);

        /// <summary>
        /// Starts tracking changes on entity in Database Context.
        /// No effect unitl context.SaveChanges() is called.
        /// </summary>
        /// <param name="entity">TEntity.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task Update(TEntity entity);

        /// <summary>
        /// Removes entity from database.
        /// No effect unitl context.SaveChanges() is called.
        /// </summary>
        /// <param name="id">Identifier.</param>
        void Delete(Guid id);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Checks if entity exists based on predicate.
        /// </summary>
        /// <param name="predicate">Predicate.</param>
        /// <param name="disableTracking"><c>true</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="ignoreQueryFilters">Ignore query filters.</param>
        /// <returns>bool.</returns>
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, bool disableTracking = true, bool ignoreQueryFilters = false);

        /// <summary>
        /// Returns entities mached by predicate.
        /// </summary>
        /// <param name="predicate">Predicate.</param>
        /// <param name="include">A function to include navigation properties.</param>
        /// <param name="disableTracking"><c>true</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="ignoreQueryFilters">Ignore query filters.</param>
        /// <returns>List{TEntity}.</returns>
        Task<List<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, bool ignoreQueryFilters = false);

        /// <summary>
        /// Returns single entity or null matched by predicate.
        /// </summary>
        /// <param name="predicate">Predicate.</param>
        /// <param name="include">A function to include navigation properties.</param>
        /// <param name="disableTracking"><c>true</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="ignoreQueryFilters">Ignore query filters.</param>
        /// <returns>TEntity.</returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, bool ignoreQueryFilters = false);

        /// <summary>
        /// Returns paged entities matched by predicate.
        /// </summary>
        /// <param name="predicate">Results filter predicate.</param>
        /// <param name="sortBy">Propery to order by.</param>
        /// <param name="page">Page to fetch.</param>
        /// <param name="pageSize">Page size to fetch.</param>
        /// <param name="sortDirection">Should order by asc or desc.</param>
        /// <param name="include">A function to include navigation properties.</param>
        /// <param name="disableTracking"><c>true</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <param name="ignoreQueryFilters">Ignore query filters.</param>
        /// <returns>TEntity list.</returns>
        Task<PagedEntities<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> predicate, string sortBy, int page, int pageSize, string sortDirection, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, bool ignoreQueryFilters = false);
    }
}
