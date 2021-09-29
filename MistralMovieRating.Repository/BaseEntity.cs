using System;

namespace MistralMovieRating.Repository
{
    public class BaseEntity : IEntity
    {
        public virtual Guid Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
