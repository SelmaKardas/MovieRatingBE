using Microsoft.EntityFrameworkCore;
using MistralMovieRating.Common;
using MistralMovieRating.Repository;
using MistralMovieRating.Repository.Entities;
using MistralMovieRating.Service.Dtos;
using MistralMovieRating.Service.Mappers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MistralMovieRating.Service
{
    public class MoviesService : IMoviesService
    {
        private IUnitOfWork<MovieRatingDBContext> unitOfWork;

        public MoviesService(IUnitOfWork<MovieRatingDBContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        private IMoviesRepository MoviesRepository => this.unitOfWork.GetRepository<IMoviesRepository>();

        private IMovieRatingsRepository MovieRatingsRepository => this.unitOfWork.GetRepository<IMovieRatingsRepository>();

        public async Task<PagedResponse<MovieTVShowDto>> GetMoviesAsync(MovieTVShowCategory category, int page, int pageSize, string search)
        {
            search ??= string.Empty;
            Expression<Func<MovieTVShow, bool>> searchCondition = x => true;

            if (!string.IsNullOrWhiteSpace(search))
            {
                if (search.ToLower().Contains("star"))
                {
                    int numberOfStars = 0;
                    string stars = new string(search.Where(Char.IsDigit).ToArray());
                    bool parseResult = int.TryParse(stars, out numberOfStars);

                    if (parseResult)
                    {
                        if (search.ToLower().Contains("at least"))
                        {
                            searchCondition = x => (x.Category == category) && (search.Contains(x.Title) ||
                                      x.Description.Contains(search) ||
                                      x.MovieTVShowActors.Any(y => search.Contains(y.Actor.FirstName) || search.Contains(y.Actor.LastName)) ||
                                      x.AverageRating >= numberOfStars);
                        }

                        else if (search.ToLower().Contains("less than"))
                        {
                            searchCondition = x => (x.Category == category) && (search.Contains(x.Title) ||
                                       x.Description.Contains(search) ||
                                       x.MovieTVShowActors.Any(y => search.Contains(y.Actor.FirstName) || search.Contains(y.Actor.LastName)) ||
                                       x.AverageRating < numberOfStars);
                        }

                        else
                        {
                            searchCondition = x => (x.Category == category) && (search.Contains(x.Title) ||
                                      x.Description.Contains(search) ||
                                      x.MovieTVShowActors.Any(y => search.Contains(y.Actor.FirstName) || search.Contains(y.Actor.LastName)) ||
                                      x.AverageRating == numberOfStars);
                        }

                        return await this.FindTopRatedMoviesByConditionAsync(searchCondition, page, pageSize);
                    }
                }

                if (search.ToLower().Contains("after"))
                {
                    int year = 0;
                    string yearString = new string(search.Where(Char.IsDigit).ToArray());
                    bool parseResult = int.TryParse(yearString, out year);

                    if (parseResult)
                    {
                        searchCondition = x => (x.Category == category) && (search.Contains(x.Title) ||
                                      x.Description.Contains(search) ||
                                      x.MovieTVShowActors.Any(y => search.Contains(y.Actor.FirstName) || search.Contains(y.Actor.LastName)) ||
                                      x.ReleaseDate.Year > year);

                        return await this.FindTopRatedMoviesByConditionAsync(searchCondition, page, pageSize);
                    }
                }

                if (search.ToLower().Contains("before"))
                {
                    int year = 0;
                  
                    string yearString = new string(search.Where(Char.IsDigit).ToArray());
                    bool parseResult = int.TryParse(yearString, out year);

                    if (parseResult)
                    {
                        searchCondition = x => (x.Category == category) && (search.Contains(x.Title) ||
                                      x.Description.Contains(search) ||
                                      x.MovieTVShowActors.Any(y => search.Contains(y.Actor.FirstName) || search.Contains(y.Actor.LastName)) ||
                                      x.ReleaseDate.Year < year);

                        return await this.FindTopRatedMoviesByConditionAsync(searchCondition, page, pageSize);
                    }
                }

                if (search.ToLower().Contains("older than"))
                {
                    int numberOfYears = 0;
              
                    string yearString = new string(search.Where(Char.IsDigit).ToArray());
                    bool parseResult = int.TryParse(yearString, out numberOfYears);

                    if (parseResult)
                    {
                        searchCondition = x => (x.Category == category) && (search.Contains(x.Title) ||
                                      x.Description.Contains(search) ||
                                      x.MovieTVShowActors.Any(y => search.Contains(y.Actor.FirstName) || search.Contains(y.Actor.LastName)) ||
                                      x.ReleaseDate.Year < DateTime.Now.Year - numberOfYears);

                        return await this.FindTopRatedMoviesByConditionAsync(searchCondition, page, pageSize);
                    }
                }


                searchCondition = x => (x.Category == category) && (x.Title.Contains(search) ||
                                      x.Description.Contains(search) ||
                                      x.MovieTVShowActors.Any(y => y.Actor.FirstName.Contains(search) || y.Actor.LastName.Contains(search)));

                return await this.FindTopRatedMoviesByConditionAsync(searchCondition, page, pageSize);

            }

            searchCondition = x => x.Category == category;
            return await this.FindTopRatedMoviesByConditionAsync(searchCondition, page, pageSize);         
        }

        public async Task<PagedResponse<MovieTVShowDto>> GetMoviesUserRatingsAsync(Guid? userId, int page, int pageSize, string search)
        {

            var movies = await this.MoviesRepository.GetMoviesUserRatingsAsync(userId, page, pageSize, search);
            var moviesDto = movies.ToDto();

            return moviesDto;
        }

        public async Task<MovieTVShowDto> GetMovieByIdAsync(Guid id)
        {
            var movie = await this.MoviesRepository.GetByIdAsync(id, x => x.Include(y => y.MovieTVShowActors).ThenInclude(z => z.Actor));
            var movieDto = movie.ToDto();

            return movieDto;
        }

      
        public async Task<MovieTVShowUserRatingDto> InsertMovieRatingAsync(MovieTVShowUserRatingDto movieTVShowUserRatingDto)
        {
            var movieTVShowUserRating = movieTVShowUserRatingDto.ToEntity();
            await this.MovieRatingsRepository.Insert(movieTVShowUserRating);
            await this.unitOfWork.CommitAsync();

            float currentMovieAverageRating = this.MovieRatingsRepository.GetMovieAverageRating(movieTVShowUserRatingDto.MovieTVShowId);
            var currentMovie = await this.GetMovieByIdAsync(movieTVShowUserRatingDto.MovieTVShowId);
            currentMovie.AverageRating = currentMovieAverageRating;

            await this.MoviesRepository.Update(currentMovie.ToEntity());
            await this.unitOfWork.CommitAsync();
            
            return movieTVShowUserRating.ToDto();
        }

        public async Task<MovieTVShowUserRatingDto> UpdateMovieRatingAsync(MovieTVShowUserRatingDto movieTVShowUserRatingDto)
        {
            var movieTVShowUserRating = movieTVShowUserRatingDto.ToEntity();
            await this.MovieRatingsRepository.Update(movieTVShowUserRating);
            await this.unitOfWork.CommitAsync();

            float currentMovieAverageRating = this.MovieRatingsRepository.GetMovieAverageRating(movieTVShowUserRatingDto.MovieTVShowId);
            var currentMovie = await this.GetMovieByIdAsync(movieTVShowUserRatingDto.MovieTVShowId);
            currentMovie.AverageRating = currentMovieAverageRating;

            await this.MoviesRepository.Update(currentMovie.ToEntity());
            await this.unitOfWork.CommitAsync();

            return movieTVShowUserRating.ToDto();
        }

        private async Task<PagedResponse<MovieTVShowDto>> FindTopRatedMoviesByConditionAsync(Expression<Func<MovieTVShow, bool>> searchCondition, int page, int pageSize)
        {
            var movies = await this.MoviesRepository.FindByConditionAsync(searchCondition, "averageRating", page, pageSize, SortDirection.Descending, x => x.Include(y => y.MovieTVShowActors).ThenInclude(z => z.Actor));
            var moviesDto = movies.ToDto();

            return moviesDto;
        }
    }
}
