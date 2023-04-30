using lobby.bll.Infrastructure.Commands.Cache;
using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using System.Security.Claims;

namespace lobby.bll.Infrastructure.Commands
{
    public class RemovePlayerCommand : IRequest<LobbyViewModel?>, ICacheableMediatrCommandResponse
    {
        public ClaimsPrincipal? User { get; init; }
        public RemovePlayerDTO RemovePlayerDTO { get; init; }
        public bool BypassCache { get; set; } = false;
        public string CacheKey => $"lobby-{RemovePlayerDTO.LobbyId}";
        public TimeSpan? SlidingExpiration { get; set; }
        public RemovePlayerCommand(RemovePlayerDTO removePlayerDTO, ClaimsPrincipal? user = null) {
            RemovePlayerDTO = removePlayerDTO;
            User = user;
        }
    }
}
