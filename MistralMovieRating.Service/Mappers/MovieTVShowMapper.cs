using AutoMapper;
using MistralMovieRating.Repository;
using MistralMovieRating.Repository.Entities;
using MistralMovieRating.Service.Dtos;
using System.Collections.Generic;

namespace MistralMovieRating.Service.Mappers
{
    public static class MovieTVShowMapper
    {
        static MovieTVShowMapper()
        {
            Mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MovieTVShowMapperProfile>();
                cfg.CreateMap<ActorDto, Actor>();
                cfg.CreateMap<Actor, ActorDto>();
                cfg.CreateMap<MovieTVShowUserRatingDto, MovieTVShowUserRating>();
                cfg.CreateMap<MovieTVShowUserRating, MovieTVShowUserRatingDto>();
            })
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static MovieTVShowDto ToDto(this MovieTVShow movie)
        {
            return Mapper.Map<MovieTVShowDto>(movie);
        }

        public static MovieTVShow ToEntity(this MovieTVShowDto movieDto)
        {
            return Mapper.Map<MovieTVShow>(movieDto);
        }

        public static IEnumerable<MovieTVShowDto> ToDto(this IEnumerable<MovieTVShow> movies)
        {
            return Mapper.Map<IEnumerable<MovieTVShowDto>>(movies);
        }

        public static PagedResponse<MovieTVShowDto> ToDto(this PagedEntities<MovieTVShow> movies)
        {
            return Mapper.Map<PagedResponse<MovieTVShowDto>>(movies);
        }
    }
}
