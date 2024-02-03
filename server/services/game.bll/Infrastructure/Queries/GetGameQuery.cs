using game.bll.Infrastructure.ViewModels;
using MediatR;
using shared.bll.Extensions;
using shared.bll.Infrastructure.Queries;
using shared.dal.Models.Cache;
using System.Security.Claims;

namespace game.bll.Infrastructure.Queries
{
    public class GetGameQuery : IRequest<GameViewModel>, ICacheableMediatrQuery
    {
        public ClaimsPrincipal? User { get; set; }
        public CacheMode CacheMode { get; set; } = CacheMode.Get;
        public string CacheKey => $"game-{User?.GetGameIdFromJwt()}";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetGameQuery(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
