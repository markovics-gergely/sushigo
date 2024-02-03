using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using shared.bll.Infrastructure.Queries;
using shared.dal.Models.Cache;
using System.Security.Claims;

namespace lobby.bll.Infrastructure.Queries
{
    public class GetLobbyQuery : IRequest<LobbyViewModel>, ICacheableMediatrQuery
    {
        public Guid LobbyId { get; private set; }
        public ClaimsPrincipal? User { get; set; }
        public CacheMode CacheMode { get; set; } = CacheMode.Get;
        public string CacheKey => $"lobby-{LobbyId}";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetLobbyQuery(Guid lobbyId, ClaimsPrincipal? user = null)
        {
            LobbyId = lobbyId;
            User = user;
        }
    }
}
