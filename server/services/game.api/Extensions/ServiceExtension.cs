using game.bll.Infrastructure;
using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.Commands.Card;
using game.bll.Infrastructure.Commands.Card.Abstract;
using game.bll.Infrastructure.ViewModels;
using game.dal;
using game.dal.UnitOfWork.Implementations;
using game.dal.UnitOfWork.Interfaces;
using MediatR;
using shared.bll.Infrastructure.Pipelines;
using shared.dal.Models;
using shared.dal.Repository.Implementations;
using shared.dal.Repository.Interfaces;

namespace game.api.Extensions
{
    public static class ServiceExtension
    {
        /// <summary>
        /// Add services for dependency injections
        /// </summary>
        /// <param name="services"></param>
        public static void AddServiceExtensions(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient(typeof(ICardCommand<EggNigiri>), typeof(EggNigiriCommand));
            services.AddTransient(typeof(ICardCommand<SalmonNigiri>), typeof(SalmonNigiriCommand));
            services.AddTransient(typeof(ICardCommand<SquidNigiri>), typeof(SquidNigiriCommand));

            services.AddTransient<IRequestHandler<PlayCardCommand>, CardCommandHandler>();
            services.AddTransient<IRequestHandler<CreateGameCommand, GameViewModel>, GameCommandHandler>();

            services.AddTransient<ISimpleAddPoint, SimpleAddPoint>();
            services.AddTransient<ISimpleAddToBoard, SimpleAddToBoard>();

            services.AddTransient(typeof(IDbContextProvider), typeof(DbContextProvider<GameDbContext>));
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
