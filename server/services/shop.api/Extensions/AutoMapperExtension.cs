using shop.bll.MappingProfiles;

namespace shop.api.Extensions
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
            services.AddAutoMapper(typeof(DeckProfile));
            services.AddAutoMapper(typeof(CardProfile));
        }
    }
}
