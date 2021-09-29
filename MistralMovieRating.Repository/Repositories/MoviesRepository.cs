using MistralMovieRating.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace MistralMovieRating.Repository
{
    public class MoviesRepository : Repository<MovieTVShow, MovieRatingDBContext>, IMoviesRepository
    {

        public MoviesRepository(MovieRatingDBContext context)
            : base(context)
        {
        }

        public async Task<PagedEntities<MovieTVShow>> GetMoviesUserRatingsAsync(Guid? userId, int page, int pageSize, string search)
        {
            search ??= string.Empty;
            Expression<Func<MovieTVShow, bool>> searchCondition = x => true;

            if (!string.IsNullOrWhiteSpace(search))
            {
                searchCondition = x => x.Title.Contains(search);
            }

            var query = this.Context.MovieTVShows
                            .AsNoTracking()
                            .Include(x => x.MovieTVShowUserRatings.Where(y => y.UserId == userId));

            return await query.PageByAsync(searchCondition, null, page, pageSize);
        }
    }
}
