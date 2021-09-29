using MistralMovieRating.Common;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace MistralMovieRating.Api.Configuration.Authorization
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        public PermissionAuthorizationHandler()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
            {
                return Task.FromResult(true);
            }

            var userClaim = context.User.FindFirst(x => x.Type == CustomClaimTypes.Permissions);

            if (userClaim == null)
            {
                return Task.FromResult(true);
            }

            var availablePermissions = JsonConvert.DeserializeObject<string[]>(userClaim.Value);

            if (requirement.Any.Length > 0 && availablePermissions.Any(x => requirement.Any.Contains(x)))
            {
                context.Succeed(requirement);
            }

            if (requirement.Except.Length > 0 && availablePermissions.Any(x => requirement.Except.Contains(x)))
            {
                context.Fail();
            }

            return Task.FromResult(true);
        }
    }
}
