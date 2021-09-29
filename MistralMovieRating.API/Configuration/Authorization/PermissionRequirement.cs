using Microsoft.AspNetCore.Authorization;

namespace MistralMovieRating.Api.Configuration.Authorization
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
  
        public PermissionRequirement(string[] any, string[] except)
        {
            this.Any = any;
            this.Except = except;
        }

        public string[] Any { get; private set; }

        public string[] Except { get; private set; }
    }
}
