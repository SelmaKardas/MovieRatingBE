using AutoMapper;
using MistralMovieRating.Repository.Entities;
using MistralMovieRating.Service.Dtos;

namespace MistralMovieRating.Service.Mappers
{
    public class ActorMapperProfile : Profile
    {
        public ActorMapperProfile()
        {
            this.CreateMap<Actor, ActorDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(src => src.LastName));

            this.CreateMap<ActorDto, Actor>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(src => src.LastName));

        }
    }
}
