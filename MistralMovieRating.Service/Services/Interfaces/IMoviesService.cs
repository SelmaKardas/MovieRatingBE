using MistralMovieRating.Common;
using MistralMovieRating.Service.Dtos;
using System;
using System.Threading.Tasks;

namespace MistralMovieRating.Service
{
    public interface IMoviesService
    {
        Task<PagedResponse<MovieTVShowDto>> GetMoviesAsync(MovieTVShowCategory category, int page, int pageSize, string search);

        Task<PagedResponse<MovieTVShowDto>> GetMoviesUserRatingsAsync(Guid? userId, int page, int pageSize, string search);

        Task<MovieTVShowDto> GetMovieByIdAsync(Guid id);

        Task<MovieTVShowUserRatingDto> InsertMovieRatingAsync(MovieTVShowUserRatingDto movieTVShowUserRatingDto);

        Task<MovieTVShowUserRatingDto> UpdateMovieRatingAsync(MovieTVShowUserRatingDto movieTVShowUserRatingDto);

    }
}
