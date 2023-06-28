using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using shared.Data.Configurations;
using shared.Data.Configurations.Implementations;
using shared.Data.Configurations.Interfaces;

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
