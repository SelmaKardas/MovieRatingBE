using MistralMovieRating.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace MistralMovieRating.Repository
{
    public partial class MovieRatingDBContext : DbContext
    {

        public MovieRatingDBContext()
            : base()
        {
        }

        public MovieRatingDBContext(DbContextOptions<MovieRatingDBContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<UserRoles> UserRoles { get; set; }

        public DbSet<MovieTVShow> MovieTVShows { get; set; }

        public DbSet<Actor> Actors { get; set; }

        public DbSet<MovieTVShowActor> MovieTVShowActors { get; set; }

        public DbSet<MovieTVShowUserRating> MovieTVShowUserRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            this.ConfigureContext(builder);
        }

        private void ConfigureContext(ModelBuilder builder)
        {

            builder.Entity<RolePermission>()
                   .HasKey(rp => new { rp.RoleId, rp.PermissionId });
            builder.Entity<RolePermission>()
                   .HasOne(r => r.Role)
                   .WithMany(rp => rp.RolePermissions)
                   .HasForeignKey(rp =>rp.RoleId);
            builder.Entity<RolePermission>()
                   .HasOne(p => p.Permission)
                   .WithMany(pr => pr.PermissionRoles)
                   .HasForeignKey(pr => pr.PermissionId);

            builder.Entity<UserRoles>()
                   .HasKey(ur => new { ur.UserId, ur.RoleId });
            builder.Entity<UserRoles>()
                   .HasOne(u => u.User)
                   .WithMany(ur => ur.UserRoles)
                   .HasForeignKey(ur => ur.UserId);
            builder.Entity<UserRoles>()
                   .HasOne(r => r.Role)
                   .WithMany(ru => ru.RoleUsers)
                   .HasForeignKey(ru => ru.RoleId);

            builder.Entity<MovieTVShowActor>()
                  .HasKey(ma => new { ma.MovieTVShowId, ma.ActorId });
            builder.Entity<MovieTVShowActor>()
                   .HasOne(m => m.MovieTVShow)
                   .WithMany(ma => ma.MovieTVShowActors)
                   .HasForeignKey(ma => ma.MovieTVShowId);
            builder.Entity<MovieTVShowActor>()
                   .HasOne(a => a.Actor)
                   .WithMany(am => am.ActorMoviesTVShows)
                   .HasForeignKey(am => am.ActorId);

        }
    }
}
