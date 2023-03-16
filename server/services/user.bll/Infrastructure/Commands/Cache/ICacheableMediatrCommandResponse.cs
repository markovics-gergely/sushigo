namespace user.bll.Infrastructure.Queries.Cache
{
    public interface ICacheableMediatrCommandResponse
    {
        string CacheKey { get; }
        TimeSpan? SlidingExpiration { get; }
    }
}
