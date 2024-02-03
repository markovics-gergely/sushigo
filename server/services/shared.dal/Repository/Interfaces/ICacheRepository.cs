namespace shared.dal.Repository.Interfaces
{
    public interface ICacheRepository
    {
        Task<T?> Get<T>(string key, CancellationToken? cancellationToken);
        Task Put(string key, object value, TimeSpan? slidingExpiration, CancellationToken? cancellationToken);
        Task Delete(string key, CancellationToken? cancellationToken);
    }
}
