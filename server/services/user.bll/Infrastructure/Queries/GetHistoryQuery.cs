using MediatR;
using shared.bll.Extensions;
using shared.bll.Infrastructure.Queries;
using System.Security.Claims;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Queries
{
    public class GetHistoryQuery : IRequest<IEnumerable<HistoryViewModel>>, ICacheableMediatrQuery
    {
        public ClaimsPrincipal? User { get; set; }
        public bool BypassCache { get; set; } = false;
        public string CacheKey => $"history-{User?.GetUserIdFromJwt()}";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetHistoryQuery(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
