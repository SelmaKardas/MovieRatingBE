using AutoMapper;
using MistralMovieRating.Repository.Entities;
using MistralMovieRating.Service.Dtos;

namespace MistralMovieRating.Service.Mappers
{
    public class MovieUserRatingMapperProfile : Profile
    {
        public MovieUserRatingMapperProfile()
        {
            this.CreateMap<MovieTVShowUserRating, MovieTVShowUserRatingDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(x => x.MovieTVShowId, opt => opt.MapFrom(src => src.MovieTVShowId))
                .ForMember(x => x.Rating, opt => opt.MapFrom(src => src.Rating));

            this.CreateMap<MovieTVShowUserRatingDto, MovieTVShowUserRating>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(x => x.MovieTVShowId, opt => opt.MapFrom(src => src.MovieTVShowId))
                .ForMember(x => x.Rating, opt => opt.MapFrom(src => src.Rating));

        }
    }
}
