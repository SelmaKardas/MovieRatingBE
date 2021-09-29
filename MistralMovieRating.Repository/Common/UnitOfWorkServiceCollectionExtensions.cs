using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MistralMovieRating.Repository.Common
{
    public static class UnitOfWorkServiceCollectionExtensions
    {
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();

            return services;
        }

        public static IServiceCollection AddRepository<TICustomRespository, TCustomRepository>(this IServiceCollection services)
            where TICustomRespository : class
            where TCustomRepository : class, TICustomRespository
        {
            services.AddScoped<TICustomRespository, TCustomRepository>();
            return services;
        }

        public static IServiceCollection AddDomainService<TIService, TService>(this IServiceCollection services)
           where TIService : class
           where TService : class, TIService
        {
            services.AddScoped<TIService, TService>();
            return services;
        }
    }
}
