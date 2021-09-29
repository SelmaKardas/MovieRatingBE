using MistralMovieRating.Repository.Entities;
using System;
using System.Linq;

namespace MistralMovieRating.Repository
{
    public class MovieRatingsRepository : Repository<MovieTVShowUserRating, MovieRatingDBContext>, IMovieRatingsRepository
    {

        public MovieRatingsRepository(MovieRatingDBContext context)
            : base(context)
        {
        }

        public float GetMovieAverageRating(Guid movieTVShowId)
        {
            var ratings = this.Context.MovieTVShowUserRatings.Where(x => x.MovieTVShowId == movieTVShowId).ToList();
            return (float)ratings.Sum(x => (float)x.Rating) / ratings.Count();
        }

    }
}