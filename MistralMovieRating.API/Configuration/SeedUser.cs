using MistralMovieRating.Repository.Entities;

namespace MistralMovieRating.API.Configuration
{
    public class SeedUser : User
    {
        public string[] Roles { get; set; }
    }
}
