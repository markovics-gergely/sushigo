using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using shared.bll.Exceptions;
using shared.bll.Infrastructure.Queries;
using shared.bll.Settings;
using System.Text;

namespace user.bll.Infrastructure.Pipelines
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheableMediatrQuery, IRequest<TResponse>
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger _logger;
        private readonly CacheSettings _settings;
        public CachingBehavior(IDistributedCache cache, ILogger<TResponse> logger, IOptions<CacheSettings> settings)
        {
            _cache = cache;
            _logger = logger;
            _settings = settings.Value;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response;
            if (request.BypassCache) return await next();
            async Task<TResponse> GetResponseAndAddToCache()
            {
                response = await next();
                var slidingExpiration = request.SlidingExpiration ?? TimeSpan.FromHours(_settings.SlidingExpiration);
                var options = new DistributedCacheEntryOptions { SlidingExpiration = slidingExpiration };
                var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response));
                await _cache.SetAsync(request.CacheKey, serializedData, options, cancellationToken);
                return response;
            }
            var cachedResponse = await _cache.GetAsync(request.CacheKey, cancellationToken);
            if (cachedResponse != null)
            {
                response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse))
                    ?? throw new EntityNotFoundException("Cache response not convertible");
                _logger.LogInformation("Fetched from Cache: '{key}'.", request.CacheKey);
            }
            else
            {
                response = await GetResponseAndAddToCache();
                _logger.LogInformation("Added to Cache: '{key}'.", request.CacheKey);
            }
            return response;
        }
    }
}
