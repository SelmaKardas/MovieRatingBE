using AutoMapper;
using MistralMovieRating.Repository.Entities;
using MistralMovieRating.Service.Dtos;
using System.Collections.Generic;

namespace MistralMovieRating.Service.Mappers
{
    public static class MovieUserRatingMapper
    {
        static MovieUserRatingMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<MovieUserRatingMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static MovieTVShowUserRatingDto ToDto(this MovieTVShowUserRating movieUserRating)
        {
            return Mapper.Map<MovieTVShowUserRatingDto>(movieUserRating);
        }

        public static MovieTVShowUserRating ToEntity(this MovieTVShowUserRatingDto movieUserRatingDto)
        {
            return Mapper.Map<MovieTVShowUserRating>(movieUserRatingDto);
        }

        public static IEnumerable<MovieTVShowUserRatingDto> ToDto(this IEnumerable<MovieTVShowUserRating> movieUserRating)
        {
            return Mapper.Map<IEnumerable<MovieTVShowUserRatingDto>>(movieUserRating);
        }
    }
}
