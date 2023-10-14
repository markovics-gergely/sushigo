using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.DataTransferObjects;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Newtonsoft.Json;

namespace game.api.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class HangfireExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IGlobalConfiguration UseMediatR(this IGlobalConfiguration configuration)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            configuration.UseSerializerSettings(jsonSettings);
            return configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddHangfireExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(conf => conf
                            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                            .UseSimpleAssemblyNameTypeSerializer()
                            .UseRecommendedSerializerSettings()
                            .UseMediatR()
                            .UseInMemoryStorage()
                /*.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero
                })*/);
            services.AddHangfireServer();
        }
    }
}
