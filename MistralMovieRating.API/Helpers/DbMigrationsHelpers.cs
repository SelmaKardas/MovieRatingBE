using MistralMovieRating.API.Configuration;
using MistralMovieRating.Common;
using MistralMovieRating.Repository;
using MistralMovieRating.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistralMovieRating.API.Helpers
{
    public static class DbMigrationsHelpers
    {
        public static async Task EnsureDatabasesMigrated(IHost host, bool seedData = false)
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                await EnsureDatabasesMigrated(services);

                if (seedData)
                {
                    await EnsureSeedData(services);
                }
            }
        }

        public static async Task EnsureDatabasesMigrated(IServiceProvider services)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<MovieRatingDBContext>())
                {
                    await context.Database.MigrateAsync();
                }
            }
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MovieRatingDBContext>();
                var seedData = scope.ServiceProvider.GetRequiredService<SeedDataConfiguration>();

                await SeedPermissions(context, seedData.Permissions);
                await SeedRoles(context, seedData.SeedRoles);
                await SeedUsers(context, seedData.SeedUsers);

                await SeedMoviesTVShows(context, seedData.SeedShows);
                await SeedActors(context, seedData.SeedActors);
                await SeedMovieActors(context, seedData.SeedMovieActors);
                await SeedMovieTVShowRatings(context, seedData.SeedRatings);

                await Task.FromResult(0);
            }
        }

        private static async Task SeedMovieTVShowRatings(MovieRatingDBContext context, SeedRatings[] ratings)
        {
            if (context.MovieTVShowUserRatings.Count() == 0)
            {
                Log.Information("Seeding movie and tv shows user ratings...");

                context.MovieTVShowUserRatings.AddRange(ratings.Select(rating => new MovieTVShowUserRating()
                {
                    UserId = rating.UserId,
                    MovieTVShowId = rating.MovieTVShowId,
                    Rating = rating.Rating
                }));
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedMoviesTVShows(MovieRatingDBContext context, SeedMoviesTVShows[] moviesTVShows)
        {
            if (context.MovieTVShows.Count() == 0)
            {
                Log.Information("Seeding movies and tv shows...");

                context.MovieTVShows.AddRange(moviesTVShows.Select(movies => new MovieTVShow()
                {
                    Id = movies.Id,
                    Title = movies.Title,
                    Description = movies.Description,
                    CoverImagePath = movies.CoverImagePath,
                    ReleaseDate = movies.ReleaseDate,
                    Category = movies.Category,
                    AverageRating = movies.AverageRating

                })) ;
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedActors(MovieRatingDBContext context, SeedActors[] actors)
        {
            if (context.Actors.Count() == 0)
            {
                Log.Information("Seeding actors...");

                context.Actors.AddRange(actors.Select(actors => new Actor()
                {
                    Id = actors.Id,
                    FirstName = actors.FirstName,
                    LastName = actors.LastName          
                }));
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedMovieActors(MovieRatingDBContext context, SeedMovieActors[] movieActors)
        {
            if (context.MovieTVShowActors.Count() == 0)
            {
                Log.Information("Seeding movie actors...");

                context.MovieTVShowActors.AddRange(movieActors.Select(movieActors => new MovieTVShowActor()
                {
                    MovieTVShowId = movieActors.MovieTVShowId,
                    ActorId = movieActors.ActorId
                }));
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedPermissions(MovieRatingDBContext context, string[] permissions)
        {
            if (context.Permissions.Count() == 0)
            {
                Log.Information("Seeding permissions");

                context.Permissions.AddRange(permissions.Select(permissionName => new Permission() { Name = permissionName }));
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedRoles(MovieRatingDBContext context, SeedRoles[] roles)
        {
            if (context.Roles.Count() == 0)
            {
                Log.Information("Seeding roles");

                context.Roles.AddRange(roles.Select(role => new Role() { Id = role.Id, Name = role.Name, Description = role.Description, BuiltInRoleType = (BuiltInRoleType)role.BuiltInRoleType }));

                foreach (var role in roles)
                {
                    var createdRole = context.Roles.Local.FirstOrDefault(x => x.Name == role.Name);
                    createdRole.RolePermissions = new List<RolePermission>();

                    foreach (var rolePermission in role.Permissions)
                    {
                        var permission = context.Permissions.IgnoreQueryFilters().FirstOrDefault(x => x.Name == rolePermission);

                        createdRole.RolePermissions.Add(new RolePermission()
                        {
                            PermissionId = permission.Id,
                        });
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedUsers(MovieRatingDBContext context, SeedUser[] users)
        {
            if (context.Users.Count() == 0)
            {
                Log.Information("Seeding users");

                context.Users.AddRange(users.Select(user => new User()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    DateOfBirth = user.DateOfBirth,
                    City = user.City,
                    State = user.State,
                    ZipCode = user.ZipCode,
                    Active = true
                }));

                foreach (var user in users)
                {
                    var createdUser = context.Users.Local.FirstOrDefault(x => x.Id == user.Id);
                    createdUser.UserRoles = new List<UserRoles>();

                    foreach (var role in user.Roles)
                    {
                        var createdRole = context.Roles.IgnoreQueryFilters().FirstOrDefault(x => x.Name == role);
                        createdUser.UserRoles.Add(new UserRoles() { RoleId = createdRole.Id });
                    }
                }

                await context.SaveChangesAsync();
            }
        }

    }
}
