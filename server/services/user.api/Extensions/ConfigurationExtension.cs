using user.dal.Configurations;
using user.dal.Configurations.Implementations;
using user.dal.Configurations.Interfaces;

namespace user.api.Extensions
{
    public static class ConfigurationExtension
    {
        public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<UserConfiguration>(configuration.GetSection("UserConfiguration"));
            services.AddTransient<IUserConfigurationService, UserConfigurationService>();
        }
    }
}
