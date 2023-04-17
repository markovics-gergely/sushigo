using lobby.dal.Configurations;
using lobby.dal.Configurations.Implementations;
using lobby.dal.Configurations.Interfaces;

namespace lobby.api.Extensions
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
            services.Configure<LobbyConfiguration>(configuration.GetSection("LobbyConfiguration"));
            services.AddTransient<ILobbyConfigurationService, LobbyConfigurationService>();
        }
    }
}
