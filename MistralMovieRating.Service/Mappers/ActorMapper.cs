using AutoMapper;
using MistralMovieRating.Repository.Entities;
using MistralMovieRating.Service.Dtos;
using System.Collections.Generic;

namespace MistralMovieRating.Service.Mappers
{
    public static class ActorMapper
    {
        static ActorMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ActorMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static ActorDto ToDto(this Actor actor)
        {
            return Mapper.Map<ActorDto>(actor);
        }

        public static Actor ToEntity(this ActorDto actorDto)
        {
            return Mapper.Map<Actor>(actorDto);
        }

        public static IEnumerable<ActorDto> ToDto(this IEnumerable<Actor> actor)
        {
            return Mapper.Map<IEnumerable<ActorDto>>(actor);
        }
    }
}
