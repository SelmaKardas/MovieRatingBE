
namespace MistralMovieRating.API.Configuration
{
    public class SeedDataConfiguration
    {
        public string[] Permissions { get; set; }

        public SeedRoles[] SeedRoles { get; set; }

        public SeedUser[] SeedUsers { get; set; }

        public SeedMoviesTVShows[] SeedShows { get; set; }

        public SeedActors[] SeedActors { get; set; }

        public SeedMovieActors[] SeedMovieActors { get; set; }

        public SeedRatings[] SeedRatings { get; set; }

    }
}
