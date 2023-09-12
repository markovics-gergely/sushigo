using game.bll.Infrastructure.ViewModels;
using MediatR;
using shared.bll.Extensions;
using shared.bll.Infrastructure.Queries;
using System.Security.Claims;

namespace game.bll.Infrastructure.Queries
{
    public class GetGameQuery : IRequest<GameViewModel>, ICacheableMediatrQuery
    {
        public ClaimsPrincipal? User { get; set; }
        public bool BypassCache { get; set; } = false;
        public string CacheKey => $"game-{User?.GetGameIdFromJwt()}";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetGameQuery(ClaimsPrincipal? user = null, bool bypass = false)
        {
            User = user;
            BypassCache = bypass;
        }
    }
}
