using Microsoft.EntityFrameworkCore;
using MistralMovieRating.Common;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace MistralMovieRating.Repository
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }
        public static async Task<PagedEntities<T>> PageByAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, string sortBy = "", int page = 0, int pageSize = 10, string sortDirection = SortDirection.Ascending)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortDirection))
            {


                query = query.OrderBy(string.Format("{0} {1}", sortBy, sortDirection));
            }

            var entities = await query.Skip((page <= 0 ? 0 : page) * pageSize).Take(pageSize).ToListAsync();
            var entitiesCount = await query.Where(predicate).CountAsync();

            PagedEntities<T> pagedEntities = new PagedEntities<T>();
            pagedEntities.Entities = entities;
            pagedEntities.TotalCount = entitiesCount;
            return pagedEntities;
        }
    }
}
