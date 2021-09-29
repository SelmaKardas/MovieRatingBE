using MistralMovieRating.Common;
using System;

namespace MistralMovieRating.Service.Dtos
{
    public class MovieTVShowUserRatingDto
    {
        public Guid? Id { get; set; }

        public Guid MovieTVShowId { get; set; }

        public Guid? UserId { get; set; }

        public MovieTVShowRating Rating { get; set; }

    }
}
