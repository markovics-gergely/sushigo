using shared.dal.Models.Cache;

namespace shared.bll.Infrastructure.Queries
{
    public interface ICacheableMediatrQuery
    {
        CacheMode CacheMode { get; set; }
        string CacheKey { get; }
        TimeSpan? SlidingExpiration { get; }
    }
}
