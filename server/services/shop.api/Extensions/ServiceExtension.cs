using MediatR;
using shop.bll.Infrastructure;
using shop.bll.Infrastructure.Commands;
using shop.bll.Infrastructure.Pipelines;
using shop.bll.Infrastructure.Queries;
using shop.bll.Infrastructure.ViewModels;
using shop.dal.Repository.Implementations;
using shop.dal.Repository.Interfaces;
using shop.dal.UnitOfWork.Implementations;
using shop.dal.UnitOfWork.Interfaces;

namespace shop.api.Extensions
{
    /// <summary>
    /// Helper class for adding services
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Add services for dependency injections
        /// </summary>
        /// <param name="services"></param>
        public static void AddServiceExtensions(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddTransient<IRequestHandler<GetDecksQuery, IEnumerable<DeckViewModel>>, ShopQueryHandler>();
            services.AddTransient<IRequestHandler<GetDeckQuery, DeckItemViewModel>, ShopQueryHandler>();

            services.AddTransient<IRequestHandler<BuyDeckCommand>, ShopCommandHandler>();
            services.AddTransient<IRequestHandler<BuyPartyCommand>, ShopCommandHandler>();

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandCachingBehavior<,>));

            services.AddDistributedMemoryCache();
        }
    }
}
