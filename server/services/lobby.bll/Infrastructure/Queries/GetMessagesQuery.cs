using lobby.bll.Infrastructure.Queries.Cache;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using System.Security.Claims;

namespace lobby.bll.Infrastructure.Queries
{
    public class GetMessagesQuery : IRequest<IEnumerable<MessageViewModel>>, ICacheableMediatrQuery
    {
        public ClaimsPrincipal? User { get; private set; }
        public Guid LobbyId { get; private set; }
        public bool BypassCache { get; set; } = false;
        public string CacheKey => $"messages-{LobbyId}";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetMessagesQuery(Guid lobbyId, ClaimsPrincipal? user = null) {
            LobbyId = lobbyId;
            User = user;
        }
    }
}
