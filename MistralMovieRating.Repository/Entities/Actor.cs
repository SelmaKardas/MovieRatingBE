using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MistralMovieRating.Repository.Entities
{
    public class Actor : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        public ICollection<MovieTVShowActor> ActorMoviesTVShows { get; set; }
    }
}
