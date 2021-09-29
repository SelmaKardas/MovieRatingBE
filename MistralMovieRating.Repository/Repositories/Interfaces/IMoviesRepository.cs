using MistralMovieRating.Repository.Entities;
using System;
using System.Threading.Tasks;

namespace MistralMovieRating.Repository
{
    public interface IMoviesRepository : IRepository<MovieTVShow>
    {
        Task<PagedEntities<MovieTVShow>> GetMoviesUserRatingsAsync(Guid? userId, int page, int pageSize, string search);
    }
}
