using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using System.Security.Claims;

namespace lobby.bll.Infrastructure.Commands
{
    public class RemovePlayerCommand : IRequest<LobbyViewModel?>
    {
        public ClaimsPrincipal? User { get; init; }
        public RemovePlayerDTO RemovePlayerDTO { get; init; }
        public RemovePlayerCommand(RemovePlayerDTO removePlayerDTO, ClaimsPrincipal? user = null) {
            RemovePlayerDTO = removePlayerDTO;
            User = user;
        }
    }
}
