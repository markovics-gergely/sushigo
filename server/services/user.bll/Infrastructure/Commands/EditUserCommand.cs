using MediatR;
using System.Security.Claims;
using user.bll.Extensions;
using user.bll.Infrastructure.DataTransferObjects;
using user.bll.Infrastructure.Queries.Cache;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Commands
{
    public class EditUserCommand : IRequest<UserViewModel>, ICacheableMediatrCommandResponse
    {
        public EditUserDTO DTO { get; }
        public ClaimsPrincipal? User { get; }
        public string CacheKey => $"user-{User?.GetUserIdFromJwt()}";
        public TimeSpan? SlidingExpiration { get; set; }

        public EditUserCommand(EditUserDTO dto, ClaimsPrincipal? user = null)
        {
            DTO = dto;
            User = user;
        }
    }
}
