using System;

namespace MistralMovieRating.Repository
{
    public interface IEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
