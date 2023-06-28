namespace shared.Logic.Infrastructure.Queries
{
    public interface ICacheableMediatrCommandResponse
    {
        string CacheKey { get; }
        TimeSpan? SlidingExpiration { get; }
    }
}
