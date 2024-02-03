using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using shared.bll.Infrastructure.Queries;
using shared.dal.Models.Cache;
using System.Security.Claims;

namespace lobby.bll.Infrastructure.Queries
{
    public class GetLobbiesQuery : IRequest<IEnumerable<LobbyItemViewModel>>, ICacheableMediatrQuery
    {
        public ClaimsPrincipal? User { get; init; }
        public CacheMode CacheMode { get; set; } = CacheMode.Get;
        public string CacheKey => "lobbies";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetLobbiesQuery(ClaimsPrincipal? user = null) {
            User = user;
        }
    }
}
