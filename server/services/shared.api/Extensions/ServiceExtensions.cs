using MediatR;
using Microsoft.Extensions.DependencyInjection;
using shared.bll.Infrastructure.Pipelines;
using shared.dal.Repository.Implementations;
using shared.dal.Repository.Interfaces;

namespace shared.api.Extensions
{
    /// <summary>
    /// Helper class for adding shared services
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Add services for dependency injections for shared services
        /// </summary>
        /// <param name="services"></param>
        public static void AddSharedServiceExtensions(this IServiceCollection services)
        {
            // Http
            services.AddHttpClient();
            services.AddHttpContextAccessor();

            // Pipelines
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

            // Providers
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IFileRepository, FileRepository>();

            // Cache
            services.AddTransient<ICacheRepository, CacheRepository>();
            services.AddDistributedMemoryCache();
        }
    }
}