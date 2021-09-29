using System;
using System.ComponentModel.DataAnnotations;

namespace MistralMovieRating.Service.Dtos
{
    public class ActorDto
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string FirstName { get; set; }

        [MaxLength(500)]
        public string LastName { get; set; }


    }
}
