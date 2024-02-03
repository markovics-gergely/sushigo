using MediatR;
using shared.bll.Extensions;
using shared.bll.Infrastructure.Queries;
using shared.dal.Models.Cache;
using System.Security.Claims;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Queries
{
    public class GetUserQuery : IRequest<UserViewModel>, ICacheableMediatrQuery
    {
        public string? Id { get; set; }
        public ClaimsPrincipal? User { get; set; }
        public CacheMode CacheMode { get; set; } = CacheMode.Get;
        public string CacheKey => $"user-{User?.GetUserIdFromJwt()}";
        public TimeSpan? SlidingExpiration { get; set; }

        public GetUserQuery(string? id, ClaimsPrincipal? user = null)
        {
            Id = id;
            User = user;
        }
    }
}
