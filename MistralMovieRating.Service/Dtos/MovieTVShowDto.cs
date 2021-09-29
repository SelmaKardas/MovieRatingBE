using MistralMovieRating.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MistralMovieRating.Service.Dtos
{
    public class MovieTVShowDto
    {
        public Guid? Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public string CoverImagePath { get; set; }

        public DateTime ReleaseDate { get; set; }

        public MovieTVShowCategory Category { get; set; }

        public float AverageRating { get; set; }

        public IEnumerable<ActorDto> Actors { get; set; }

        public string FullActors => string.Join(", ", Actors.ToArray().Select(x => $"{x.FirstName} {x.LastName}"));
      
        public IEnumerable<MovieTVShowUserRatingDto> MovieUserRatings { get; set; }

        public MovieTVShowUserRatingDto UserRating => MovieUserRatings.FirstOrDefault();

    }
}
