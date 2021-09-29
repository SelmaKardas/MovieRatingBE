using MistralMovieRating.API.Configuration;
using MistralMovieRating.API.Configuration.Constants;
using MistralMovieRating.Common;
using MistralMovieRating.Service;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.Security.Claims;
namespace MistralMovieRating.API.Helpers
{
    public static class StartupHelpers
    {
        public static void AddApiAuthentication(this IServiceCollection services, ApiConfiguration apiConfiguration)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = apiConfiguration.IdentityServerBaseUrl;
                    options.Audience = "api";
                    options.RequireHttpsMetadata = false;

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async ctx =>
                        {
                            string userId = ctx.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

                            if (!string.IsNullOrEmpty(userId))
                            {
                                var usersService = ctx.HttpContext.RequestServices.GetRequiredService<IUsersService>();
                                var permissions = await usersService.GetUserPermissionsAsync(userId.ToGuid());

                                var claims = new List<Claim>
                                {
                                    new Claim(CustomClaimTypes.Permissions, JsonConvert.SerializeObject(permissions)),
                                };
                                var appIdentity = new ClaimsIdentity(claims);

                                ctx.Principal.AddIdentity(appIdentity);
                            }
                            else
                            {
                                Log.Warning("Failed to load permissions for request", ctx.ToString());
                            }
                        },
                    };
                });
        }

        public static ApiConfiguration AddApplicationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var apiConfiguration = configuration.GetSection(nameof(ApiConfiguration)).Get<ApiConfiguration>();
            services.AddSingleton(apiConfiguration);

            SeedDataConfiguration seedDataConfiguration = new SeedDataConfiguration();
            configuration.GetSection(ConfigurationConstants.SeedDataConfigurationKey).Bind(seedDataConfiguration);
            services.AddSingleton(seedDataConfiguration);

            return apiConfiguration;
        }
    }
}
