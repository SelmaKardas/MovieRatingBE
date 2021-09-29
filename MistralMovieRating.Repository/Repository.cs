using MistralMovieRating.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace MistralMovieRating.Repository
{
    public abstract class Repository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : BaseEntity
        where TContext : DbContext
    {
        private DbSet<TEntity> entities;

        public Repository(TContext context)
        {
            this.Context = context ?? throw new ArgumentException(nameof(context));
            this.entities = context.Set<TEntity>();
        }

        protected TContext Context { get; }

        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await this.entities.AddAsync(entity);
            return entity;
        }

        public virtual Task Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            this.Context.Update(entity);

            return Task.FromResult(0);
        }

        public void Delete(TEntity entity) => this.Context.Remove(entity);

        public virtual void Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var typeInfo = typeof(TEntity).GetTypeInfo();
            var key = this.Context.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name);
            if (property != null)
            {
                var entity = Activator.CreateInstance<TEntity>();
                property.SetValue(entity, id);
                this.Context.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = this.Context.Find(typeof(TEntity), id) as TEntity;
                if (entity != null)
                {
                    this.Delete(entity);
                }
            }
        }

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = this.entities;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return query.AnyAsync(predicate);
        }

        public Task<List<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = this.entities;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return query.ToListAsync();
        }

        public Task<TEntity> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            IQueryable<TEntity> query = this.entities;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            if (predicate == null)
            {
                throw new ArgumentException(nameof(predicate));
            }

            IQueryable<TEntity> query = this.entities;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return query.Where(predicate).ToListAsync();
        }

        public Task<PagedEntities<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> predicate, string sortBy, int page = 0, int pageSize = 10, string sortDirection = SortDirection.Ascending, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            if (predicate == null)
            {
                throw new ArgumentException(nameof(predicate));
            }

            IQueryable<TEntity> query = this.entities;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return query.PageByAsync(predicate, sortBy, page, pageSize, sortDirection);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = this.entities;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return query.FirstOrDefaultAsync(predicate);
        }
    }
}
