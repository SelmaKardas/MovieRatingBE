using MistralMovieRating.Api.Configuration.Authorization;
using MistralMovieRating.API.Configuration;
using MistralMovieRating.API.Helpers;
using MistralMovieRating.Repository;
using MistralMovieRating.Repository.Common;
using MistralMovieRating.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace MistralMovieRating.API
{
    public class Startup
    {
        private ApiConfiguration apiConfiguration;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            this.apiConfiguration = services.AddApplicationConfiguration(this.Configuration);
         
            services.AddHttpContextAccessor();
            services.AddHealthChecks();
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ApiValidationFilterAttribute));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", corsBuilder =>
                {
                    corsBuilder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(origin => origin == this.apiConfiguration.OriginAllowed)
                    .AllowCredentials();
                });
            });

            this.AddAuthentication(services, this.apiConfiguration);

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            this.RegisterDbContext(services);
            services.AddScoped<MovieRatingDBContext>();
            services.AddUnitOfWork<MovieRatingDBContext>();


            services.AddRepository<IUsersRepository, UsersRepository>();
            services.AddRepository<IRolesRepository, RolesRepository>();
            services.AddRepository<IPermissionsRepository, PermissionsRepository>();
            services.AddRepository<IMoviesRepository, MoviesRepository>();
            services.AddRepository<IMovieRatingsRepository, MovieRatingsRepository>();

            services.AddDomainService<IUsersService, UsersService>();
            services.AddDomainService<IMoviesService, MoviesService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApiConfiguration apiConfiguration)
        {
            
            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });


            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health").WithMetadata(new AllowAnonymousAttribute());
            });
        }

        public virtual void RegisterDbContext(IServiceCollection services)
        {
            services.AddDbContext<MovieRatingDBContext>(options => options.UseSqlServer(this.apiConfiguration.DatabaseConnectionString));
        }

        public virtual void AddAuthentication(IServiceCollection services, ApiConfiguration apiConfiguration)
        {
            services.AddApiAuthentication(apiConfiguration);
        }
    }
}
