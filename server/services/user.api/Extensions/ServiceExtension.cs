using MediatR;
using shared.bll.Infrastructure.Pipelines;
using shared.dal.Repository.Implementations;
using shared.dal.Repository.Interfaces;
using user.bll.Infrastructure;
using user.bll.Infrastructure.Commands;
using user.bll.Infrastructure.Events;
using user.bll.Infrastructure.Queries;
using user.bll.Infrastructure.ViewModels;
using user.dal;
using user.dal.UnitOfWork.Implementations;
using user.dal.UnitOfWork.Interfaces;

namespace user.api.Extensions
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
            services.AddTransient<IRequestHandler<CreateUserCommand, bool>, UserCommandHandler>();
            services.AddTransient<IRequestHandler<EditUserCommand, UserViewModel>, UserCommandHandler>();
            services.AddTransient<IRequestHandler<RemoveUserCommand>, UserCommandHandler>();
            services.AddTransient<IRequestHandler<ClaimPartyCommand, UserViewModel>, UserCommandHandler>();
            services.AddTransient<IRequestHandler<ClaimDeckCommand, UserViewModel>, UserCommandHandler>();
            services.AddTransient<IRequestHandler<EditUserRoleCommand>, UserCommandHandler>();
            services.AddTransient<IRequestHandler<JoinLobbyCommand, UserViewModel>, UserCommandHandler>();
            services.AddTransient<IRequestHandler<JoinGameCommand, UserViewModel>, UserCommandHandler>();
            services.AddTransient<IRequestHandler<EndGameCommand, UserViewModel>, UserCommandHandler>();

            services.AddTransient<IRequestHandler<AddFriendCommand, UserNameViewModel>, FriendCommandHandler>();
            services.AddTransient<IRequestHandler<RemoveFriendCommand>, FriendCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateFriendOfflineCommand>, FriendCommandHandler>();

            services.AddTransient<IRequestHandler<GetFriendsQuery, FriendListViewModel>, FriendQueryHandler>();

            services.AddTransient<IRequestHandler<GetUserQuery, UserViewModel>, UserQueryHandler>();
            services.AddTransient<IRequestHandler<GetUserByIdQuery, UserViewModel>, UserQueryHandler>();
            services.AddTransient<IRequestHandler<GetUsersByRoleQuery, IEnumerable<UserNameViewModel>>, UserQueryHandler>();
            services.AddTransient<IRequestHandler<GetHistoryQuery, IEnumerable<HistoryViewModel>>, UserQueryHandler>();

            services.AddTransient<INotificationHandler<RefreshHistoryEvent>, HistoryNotificationHandler>();

            services.AddTransient(typeof(IDbContextProvider), typeof(DbContextProvider<UserDbContext>));
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

            services.AddDistributedMemoryCache();
        }
    }
}
