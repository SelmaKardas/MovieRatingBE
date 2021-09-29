using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace MistralMovieRating.Api.Configuration.Authorization
{
    internal class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        private const string POLICYPREFIX = "Permissions.";
        public PermissionAuthorizeAttribute(string[] any = null, string[] except = null)
        {
            any ??= new string[] { };
            except ??= new string[] { };

            this.Policy = POLICYPREFIX
            + JsonConvert.SerializeObject(new { any, except });
        }
    }
}
