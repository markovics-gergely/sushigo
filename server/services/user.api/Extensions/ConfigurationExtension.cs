using user.dal.Configurations;
using user.dal.Configurations.Implementations;
using user.dal.Configurations.Interfaces;

namespace user.api.Extensions
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
            services.Configure<UserConfiguration>(configuration.GetSection("UserConfiguration"));
            services.AddTransient<IUserConfigurationService, UserConfigurationService>();
        }
    }
}
