using game.bll.Infrastructure;
using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.Commands.Card;
using game.bll.Infrastructure.Commands.Card.Utils;
using game.bll.Infrastructure.Commands.Card.Utils.Implementations;
using game.bll.Infrastructure.Events;
using game.bll.Infrastructure.Queries;
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
    /// <summary>
    /// 
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
            services.AddHttpContextAccessor();

            services.AddTransient(typeof(ICardCommand<EggNigiri>), typeof(EggNigiriCommand));
            services.AddTransient(typeof(ICardCommand<SalmonNigiri>), typeof(SalmonNigiriCommand));
            services.AddTransient(typeof(ICardCommand<SquidNigiri>), typeof(SquidNigiriCommand));
            services.AddTransient(typeof(ICardCommand<MakiRoll>), typeof(MakiCommand));
            services.AddTransient(typeof(ICardCommand<Temaki>), typeof(TemakiCommand));
            services.AddTransient(typeof(ICardCommand<Uramaki>), typeof(UramakiCommand));
            services.AddTransient(typeof(ICardCommand<Dumpling>), typeof(DumplingCommand));
            services.AddTransient(typeof(ICardCommand<Edamame>), typeof(EdamameCommand));
            services.AddTransient(typeof(ICardCommand<Eel>), typeof(EelCommand));
            services.AddTransient(typeof(ICardCommand<Onigiri>), typeof(OnigiriCommand));
            services.AddTransient(typeof(ICardCommand<MisoSoup>), typeof(MisoCommand));
            services.AddTransient(typeof(ICardCommand<Sashimi>), typeof(SashimiCommand));
            services.AddTransient(typeof(ICardCommand<Tempura>), typeof(TempuraCommand));
            services.AddTransient(typeof(ICardCommand<Tofu>), typeof(TofuCommand));
            services.AddTransient(typeof(ICardCommand<Chopsticks>), typeof(ChopSticksCommand));
            services.AddTransient(typeof(ICardCommand<Menu>), typeof(MenuCommand));
            services.AddTransient(typeof(ICardCommand<SoySauce>), typeof(SoySauceCommand));
            services.AddTransient(typeof(ICardCommand<Spoon>), typeof(SpoonCommand));
            services.AddTransient(typeof(ICardCommand<SpecialOrder>), typeof(SpecialOrderCommand));
            services.AddTransient(typeof(ICardCommand<TakeoutBox>), typeof(TakeoutBoxCommand));
            services.AddTransient(typeof(ICardCommand<Tea>), typeof(TeaCommand));
            services.AddTransient(typeof(ICardCommand<Wasabi>), typeof(WasabiCommand));
            services.AddTransient(typeof(ICardCommand<GreenTeaIceCream>), typeof(GreenTeaIceCreamCommand));
            services.AddTransient(typeof(ICardCommand<Fruit>), typeof(FruitCommand));
            services.AddTransient(typeof(ICardCommand<Pudding>), typeof(PuddingCommand));

            services.AddTransient<IRequestHandler<PlayCardCommand>, CardCommandHandler>();
            services.AddTransient<IRequestHandler<SkipAfterTurnCommand>, CardCommandHandler>();
            services.AddTransient<IRequestHandler<PlayAfterTurnCommand>, CardCommandHandler>();
            services.AddTransient<IRequestHandler<CreateGameCommand, GameViewModel>, GameCommandHandler>();
            services.AddTransient<IRequestHandler<ProceedEndTurnCommand>, GameCommandHandler>();
            services.AddTransient<IRequestHandler<ProceedEndRoundCommand>, GameCommandHandler>();
            services.AddTransient<IRequestHandler<RemoveGameCommand>, GameCommandHandler>();

            services.AddTransient<IRequestHandler<GetGameQuery, GameViewModel>, GameQueryHandler>();
            services.AddTransient<IRequestHandler<GetHandQuery, HandViewModel>, GameQueryHandler>();
            services.AddTransient<IRequestHandler<GetOwnHandQuery, HandViewModel>, GameQueryHandler>();

            services.AddTransient<INotificationHandler<RefreshGameEvent>, GameNotificationHandler>();

            services.AddTransient(typeof(IDbContextProvider), typeof(DbContextProvider<GameDbContext>));
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandCachingBehavior<,>));

            services.AddTransient(typeof(ISimpleAddPoint), typeof(SimpleAddPoint));
            services.AddTransient(typeof(ISimpleAddToBoard), typeof(SimpleAddToBoard));
            services.AddTransient(typeof(IAddPointByDelegate), typeof(AddPointByDelegate));

            services.AddDistributedMemoryCache();
        }
    }
}
