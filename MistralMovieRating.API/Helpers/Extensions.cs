using System;
using System.Security.Claims;

namespace MistralMovieRating.Api.Helpers
{
    public static class Extensions
    {
 
        public static Guid? GetUserId(this ClaimsPrincipal principal)
        {
            var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Guid.TryParse(id, out var userId))
            {
                return userId;
            }

            else 
            {
                return null;
            }
        }
    }
}
