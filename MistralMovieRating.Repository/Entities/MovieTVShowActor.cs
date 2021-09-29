using System;

namespace MistralMovieRating.Repository.Entities
{
    public class MovieTVShowActor
    {
        public Guid MovieTVShowId { get; set; }

        public Guid ActorId { get; set; }

        public MovieTVShow MovieTVShow { get; set; }

        public Actor Actor { get; set; }
    }
}
