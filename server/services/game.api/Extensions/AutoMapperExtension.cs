using game.bll.MappingProfiles;

namespace game.api.Extensions
{
    /// <summary>
    /// Manage automapper configurations
    /// </summary>
    public static class AutoMapperExtension
    {
        /// <summary>
        /// Add automapper profile configurations
        /// </summary>
        /// <param name="services"></param>
        public static void AddAutoMapperExtensions(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CardProfile));
            services.AddAutoMapper(typeof(PlayerProfile));
            services.AddAutoMapper(typeof(GameProfile));
        }
    }
}
