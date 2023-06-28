using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using shared.dal.Configurations;
using shared.dal.Configurations.Implementations;
using shared.dal.Configurations.Interfaces;

namespace shared.Api.Extensions
{
    /// <summary>
    /// Configuration related extensions
    /// </summary>
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Add configurations
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileConfiguration>(configuration.GetSection("FileConfiguration"));
            services.AddTransient<IFileConfigurationService, FileConfigurationService>();
        }
    }
}
