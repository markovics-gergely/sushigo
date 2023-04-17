namespace lobby.bll.Infrastructure.Commands.Cache
{
    public interface ICacheableMediatrCommandResponse
    {
        string CacheKey { get; }
        TimeSpan? SlidingExpiration { get; }
    }
}
