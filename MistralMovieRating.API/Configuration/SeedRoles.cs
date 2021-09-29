using System;

namespace MistralMovieRating.API.Configuration
{
    public class SeedRoles
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? BuiltInRoleType { get; set; }

        public string[] Permissions { get; set; }
    }
}
