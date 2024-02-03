using MediatR;
using lobby.bll.Infrastructure;
using lobby.bll.Infrastructure.Commands;
using lobby.bll.Infrastructure.Queries;
using lobby.bll.Infrastructure.ViewModels;
using lobby.dal.UnitOfWork.Implementations;
using lobby.dal.UnitOfWork.Interfaces;
using lobby.bll.Infrastructure.Events;
using shared.dal.Repository.Implementations;
using shared.dal.Repository.Interfaces;
using lobby.dal;
using shared.bll.Infrastructure.Pipelines;

namespace lobby.api.Extensions
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

            // Lobby Commands
            services.AddTransient<IRequestHandler<CreateLobbyCommand, LobbyViewModel>, LobbyCommandHandler>();
            services.AddTransient<IRequestHandler<JoinLobbyCommand, LobbyViewModel>, LobbyCommandHandler>();
            services.AddTransient<IRequestHandler<AddPlayerCommand, LobbyViewModel>, LobbyCommandHandler>();
            services.AddTransient<IRequestHandler<RemovePlayerCommand, LobbyViewModel?>, LobbyCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateLobbyDeckCommand, LobbyViewModel>, LobbyCommandHandler>();
            services.AddTransient<IRequestHandler<PlayerReadyCommand, LobbyViewModel>, LobbyCommandHandler>();
            services.AddTransient<IRequestHandler<RemoveLobbyCommand, LobbyViewModel?>, LobbyCommandHandler>();

            // Message Queries
            services.AddTransient<IRequestHandler<GetMessagesQuery, IEnumerable<MessageViewModel>>, MessageQueryHandler>();
            services.AddTransient<IRequestHandler<CreateMessageCommand, MessageViewModel>, MessageCommandHandler>();

            // Lobby Queries
            services.AddTransient<IRequestHandler<GetLobbiesQuery, IEnumerable<LobbyItemViewModel>>, LobbyQueryHandler>();
            services.AddTransient<IRequestHandler<GetLobbyQuery, LobbyViewModel>, LobbyQueryHandler>();

            // Lobby Events
            services.AddTransient<INotificationHandler<PlayerReadyEvent>, LobbyEventHandler>();
            services.AddTransient<INotificationHandler<AddLobbyEvent>, LobbyEventHandler>();

            // Message Events
            services.AddTransient<INotificationHandler<RemoveLobbyEvent>, LobbyEventHandler>();
            services.AddTransient<INotificationHandler<AddMessageEvent>, MessageEventHandler>();

            // Providers
            services.AddTransient(typeof(IDbContextProvider), typeof(DbContextProvider<LobbyDbContext>));
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            // Pipelines
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

            services.AddDistributedMemoryCache();
        }
    }
}
