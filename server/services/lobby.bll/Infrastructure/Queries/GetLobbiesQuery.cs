using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using shared.bll.Infrastructure.Queries;
using System.Security.Claims;

namespace lobby.bll.Infrastructure.Queries
{
    public class GetLobbiesQuery : IRequest<IEnumerable<LobbyItemViewModel>>, ICacheableMediatrQuery
    {
        public ClaimsPrincipal? User { get; init; }
        public bool BypassCache { get; set; } = false;
        public string CacheKey => "lobbies";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetLobbiesQuery(ClaimsPrincipal? user = null) {
            User = user;
        }
    }
}
