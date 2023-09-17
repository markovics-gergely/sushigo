using MediatR;
using shared.bll.Extensions;
using System.Security.Claims;
using user.bll.Infrastructure.Commands.Cache;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Commands
{
    public class RemoveUserCommand : IRequest<UserViewModel?>, ICacheableMediatrCommandResponse
    {
        public ClaimsPrincipal? User { get; }
        public string CacheKey => $"user-{User?.GetUserIdFromJwt()}";
        public TimeSpan? SlidingExpiration { get; set; }
        public RemoveUserCommand(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
