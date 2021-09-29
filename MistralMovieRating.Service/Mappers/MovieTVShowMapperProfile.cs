using AutoMapper;
using MistralMovieRating.Repository;
using MistralMovieRating.Repository.Entities;
using MistralMovieRating.Service.Dtos;
using System.Linq;

namespace MistralMovieRating.Service.Mappers
{
    public class MovieTVShowMapperProfile : Profile
    {
        public MovieTVShowMapperProfile()
        {
            this.CreateMap<MovieTVShow, MovieTVShowDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(x => x.CoverImagePath, opt => opt.MapFrom(src => src.CoverImagePath))
                .ForMember(x => x.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(x => x.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate))
                .ForMember(x => x.Actors, opt => opt.MapFrom(src => src.MovieTVShowActors.Select(x => x.Actor)))
                .ForMember(x => x.MovieUserRatings, opt => opt.MapFrom(src => src.MovieTVShowUserRatings));


            this.CreateMap<MovieTVShowDto, MovieTVShow>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(x => x.CoverImagePath, opt => opt.MapFrom(src => src.CoverImagePath))
                .ForMember(x => x.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(x => x.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate));

            this.CreateMap<PagedEntities<MovieTVShow>, PagedResponse<MovieTVShowDto>>()
               .ForMember(x => x.Entities, opt => opt.MapFrom(src => src.Entities));
        }
    }
}
