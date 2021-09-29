using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MistralMovieRating.Api.Configuration.Authorization
{
    internal class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private const string POLICYPREFIX = "Permissions.";
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            this.FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => this.FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICYPREFIX, StringComparison.OrdinalIgnoreCase))
            {
                var policy = policyName.Replace(POLICYPREFIX, string.Empty);

                var permissions = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(policy);

                var authorizationPolicy = new AuthorizationPolicyBuilder();
                authorizationPolicy.AddRequirements(new PermissionRequirement(permissions["any"], permissions["except"]));

                return Task.FromResult(authorizationPolicy.Build());
            }

            // Policy is not for permissions, try the default provider.
            return this.FallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        /// <summary>
        /// Fallback policy.
        /// </summary>
        /// <returns>Returns fallback policy.</returns>
        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => this.FallbackPolicyProvider.GetDefaultPolicyAsync();
    }
}
