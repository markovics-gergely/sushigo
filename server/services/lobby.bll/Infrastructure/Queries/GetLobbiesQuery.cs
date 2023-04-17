using lobby.bll.Infrastructure.Queries.Cache;

namespace lobby.bll.Infrastructure.Queries
{
    public class GetLobbiesQuery : ICacheableMediatrQuery
    {
        public bool BypassCache { get; set; }
        public string CacheKey => "lobbies";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetLobbiesQuery(bool bypassCache = false) {
            BypassCache = bypassCache;
        }
    }
}
