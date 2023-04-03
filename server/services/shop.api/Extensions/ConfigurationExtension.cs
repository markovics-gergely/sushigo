using shop.dal.Configurations;
using shop.dal.Configurations.Implementations;
using shop.dal.Configurations.Interfaces;

namespace shop.api.Extensions
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
            services.Configure<ShopConfiguration>(configuration.GetSection("ShopConfiguration"));
            services.AddTransient<IShopConfigurationService, ShopConfigurationService>();
        }
    }
}
