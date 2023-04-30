using lobby.bll.Infrastructure.Queries.Cache;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;

namespace lobby.bll.Infrastructure.Queries
{
    public class GetLobbiesQuery : IRequest<IEnumerable<LobbyItemViewModel>>, ICacheableMediatrQuery
    {
        public bool BypassCache { get; set; }
        public string CacheKey => "lobbies";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetLobbiesQuery(bool bypassCache = false) {
            BypassCache = bypassCache;
        }
    }
}
