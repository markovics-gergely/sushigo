using MediatR;
using Microsoft.AspNetCore.Http;
using shared.bll.Infrastructure.Queries;
using shared.dal.Models.Cache;
using shared.dal.Repository.Interfaces;

namespace shared.bll.Infrastructure.Pipelines
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheableMediatrQuery, IRequest<TResponse>
    {
        private readonly ICacheRepository _cacheRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CachingBehavior(ICacheRepository cacheRepository, IHttpContextAccessor httpContextAccessor)
        {
            _cacheRepository = cacheRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        private void OverrideCacheMode(TRequest request)
        {
            var cacheModeFromQuery = _httpContextAccessor.HttpContext?.Request.Query["cache"];
            Enum.TryParse(typeof(CacheMode), cacheModeFromQuery, out var cacheMode);
            if (cacheMode != null)
            {
                request.CacheMode = (CacheMode)cacheMode;
            }
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            OverrideCacheMode(request);
            if (request.CacheMode == CacheMode.Delete)
            {
                await _cacheRepository.Delete(request.CacheKey, cancellationToken);
                return await next();
            }
            if (request.CacheMode.IsFetch())
            {
                var cached = await _cacheRepository.Get<TResponse>(request.CacheKey, cancellationToken);
                if (cached != null)
                {
                    return cached;
                }
            }
            TResponse response = await next();
            if (request.CacheMode.IsStore() && response != null)
            {
                await _cacheRepository.Put(request.CacheKey, response, request.SlidingExpiration, cancellationToken);
            }
            return response;
        }
    }
}
