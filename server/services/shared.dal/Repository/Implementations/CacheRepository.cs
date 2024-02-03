using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using shared.dal.Repository.Interfaces;
using shared.dal.Settings;
using System.Text;

namespace shared.dal.Repository.Implementations
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDistributedCache _cache;
        private readonly CacheSettings _settings;
        private readonly ILogger _logger;
        public CacheRepository(IDistributedCache cache, IOptions<CacheSettings> settings, ILogger<CacheRepository> logger)
        {
            _cache = cache;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task Delete(string key, CancellationToken? cancellationToken)
        {
            await _cache.RemoveAsync(key, cancellationToken ?? CancellationToken.None);
            _logger.LogInformation("[{key}] Removed from cache", key);
        }

        public async Task<T?> Get<T>(string key, CancellationToken? cancellationToken)
        {
            var cachedResponse = await _cache.GetAsync(key, cancellationToken ?? CancellationToken.None);
            if (cachedResponse != null)
            {
                var item = JsonConvert.DeserializeObject<T>(Encoding.Default.GetString(cachedResponse));
                _logger.LogInformation("[{key}] Fetched from cache", key);
                return item;
            }
            _logger.LogInformation("[{key}] Not found in cache", key);
            return default;
        }

        public async Task Put(string key, object value, TimeSpan? slidingExpiration, CancellationToken? cancellationToken)
        {
            var options = new DistributedCacheEntryOptions { SlidingExpiration = slidingExpiration ?? TimeSpan.FromHours(_settings.SlidingExpiration) };
            var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(value));
            await _cache.SetAsync(key, serializedData, options, cancellationToken ?? CancellationToken.None);
            _logger.LogInformation("[{key}] Added to cache", key);
        }
    }
}
