using MistralMovieRating.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MistralMovieRating.Repository.Entities
{
    public class MovieTVShow : BaseEntity
    {
        [Required]
        [MaxLength(120)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public string CoverImagePath { get; set; }

        public DateTime ReleaseDate { get; set; }

        public MovieTVShowCategory Category { get; set; }

        public float AverageRating { get; set; }

        public ICollection<MovieTVShowActor> MovieTVShowActors { get; set; }

        public ICollection<MovieTVShowUserRating> MovieTVShowUserRatings { get; set; }

    }
}
