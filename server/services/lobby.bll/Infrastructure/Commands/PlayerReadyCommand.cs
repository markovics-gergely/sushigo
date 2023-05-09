using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using System.Security.Claims;

namespace lobby.bll.Infrastructure.Commands
{
    public class PlayerReadyCommand : IRequest<LobbyViewModel>
    {
        public ClaimsPrincipal? User { get; init; }
        public PlayerReadyDTO PlayerReadyDTO { get; init; }

        public PlayerReadyCommand(PlayerReadyDTO playerReadyDTO, ClaimsPrincipal? user = null)
        {
            User = user;
            PlayerReadyDTO = playerReadyDTO;
        }
    }
}
