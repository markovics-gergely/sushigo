using game.api.Extensions;
using Hangfire;
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
                            .UseInMemoryStorage());
            services.AddHangfireServer();
        }

        /*public static void UseJobs(IMediator mediator, long jobId, CreateJobDTO createJobDTO)
        {
            if (createJobDTO.PeriodType == null || createJobDTO.PeriodType == PeriodType.Nothing)
            {
                mediator.Schedule($"{jobId} - {createJobDTO.Name}", new UseJobCommand(jobId), createJobDTO.SchemedTransaction.DateTime);
            }
            else
            {
                mediator.AddOrUpdate($"{jobId} - {createJobDTO.Name}", new UseJobCommand(jobId), GetCronByPeriodType(createJobDTO.PeriodType, createJobDTO.StartTime));
            }
        }*/
    }
}
