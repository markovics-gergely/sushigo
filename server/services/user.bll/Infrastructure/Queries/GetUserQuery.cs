using MediatR;
using System.Security.Claims;
using user.bll.Extensions;
using user.bll.Infrastructure.Queries.Cache;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Queries
{
    public class GetUserQuery : IRequest<UserViewModel>, ICacheableMediatrQuery
    {
        public string? Id { get; set; }
        public ClaimsPrincipal? User { get; set; }
        public bool BypassCache { get; set; } = false;
        public string CacheKey => $"user-{User?.GetUserIdFromJwt()}";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetUserQuery(string? id, ClaimsPrincipal? user = null)
        {
            Id = id;
            User = user;
        }
    }
}
