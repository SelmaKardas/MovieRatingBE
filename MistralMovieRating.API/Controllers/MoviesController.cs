using MistralMovieRating.Service;
using MistralMovieRating.Service.Dtos;
using Microsoft.AspNetCore.Mvc;
using MistralMovieRating.Api.Configuration.Authorization;
using System;
using System.Threading.Tasks;
using MistralMovieRating.Common;
using Microsoft.AspNetCore.Authorization;
using MistralMovieRating.Api.Helpers;

namespace MistralMovieRating.Api.Controllers
{
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        private IMoviesService moviesService;

        public MoviesController(IMoviesService moviesService)
        {
            this.moviesService = moviesService;
        }


        [HttpGet]
        [AllowAnonymous]
        public Task<PagedResponse<MovieTVShowDto>> Get(MovieTVShowCategory category, string search, int page, int pageSize)
        {
            return this.moviesService.GetMoviesAsync(category, page, pageSize, search);
        }

        [HttpGet("rating")]
        [AllowAnonymous]
        public Task<PagedResponse<MovieTVShowDto>> GetMoviesUserRatings(string search, int page, int pageSize)
        {
            var movies = this.moviesService.GetMoviesUserRatingsAsync(this.User.GetUserId(), page, pageSize, search);
            return movies;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMovieById(Guid id)
        {
            var movie = await this.moviesService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return this.NotFound();
            }

            return this.Ok(movie);
        }

        [HttpPost("rating")]
        [PermissionAuthorize(any: new string[]
        {
            Permissions.Rating.Create,
        })]
        public async Task<IActionResult> AddMovieRating([FromBody] MovieTVShowUserRatingDto movieTVShowUserRatingDto)
        {
            movieTVShowUserRatingDto.UserId = this.User.GetUserId();
            movieTVShowUserRatingDto = await this.moviesService.InsertMovieRatingAsync(movieTVShowUserRatingDto);

            return this.Ok(movieTVShowUserRatingDto);
        }



        [HttpPut("rating")]
        [PermissionAuthorize(any: new string[]
        {
            Permissions.Rating.Edit,
        })]
        public async Task<IActionResult> UpdateMovieRating([FromBody] MovieTVShowUserRatingDto movieTVShowUserRatingDto)
        {
            movieTVShowUserRatingDto.UserId = this.User.GetUserId();
            movieTVShowUserRatingDto = await this.moviesService.UpdateMovieRatingAsync(movieTVShowUserRatingDto);

            return this.Ok(movieTVShowUserRatingDto);
        }

    }
}
