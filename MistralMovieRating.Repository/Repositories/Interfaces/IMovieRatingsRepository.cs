using MistralMovieRating.Repository.Entities;
using System;

namespace MistralMovieRating.Repository
{
    public interface IMovieRatingsRepository : IRepository<MovieTVShowUserRating>
    {
        float GetMovieAverageRating(Guid movieTVShowId);
    }
}
