using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using shared.bll.Infrastructure.Queries;
using shared.dal.Models.Cache;
using System.Security.Claims;

namespace lobby.bll.Infrastructure.Queries
{
    public class GetMessagesQuery : IRequest<IEnumerable<MessageViewModel>>, ICacheableMediatrQuery
    {
        public ClaimsPrincipal? User { get; private set; }
        public Guid LobbyId { get; private set; }
        public CacheMode CacheMode { get; set; } = CacheMode.Get;
        public string CacheKey => $"messages-{LobbyId}";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetMessagesQuery(Guid lobbyId, ClaimsPrincipal? user = null) {
            LobbyId = lobbyId;
            User = user;
        }
    }
}
