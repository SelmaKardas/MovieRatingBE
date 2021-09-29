using MistralMovieRating.Common;
using System;

namespace MistralMovieRating.Repository.Entities
{
    public class MovieTVShowUserRating : BaseEntity
    {
        public Guid MovieTVShowId { get; set; }

        public Guid UserId { get; set; }

        public MovieTVShow MovieTVShow { get; set; }

        public User User { get; set; }

        public MovieTVShowRating Rating { get; set; }
    }
}
