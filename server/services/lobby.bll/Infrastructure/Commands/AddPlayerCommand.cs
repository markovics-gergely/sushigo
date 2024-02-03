using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using System.Security.Claims;

namespace lobby.bll.Infrastructure.Commands
{
    public class AddPlayerCommand : IRequest<LobbyViewModel>
    {
        public ClaimsPrincipal? User { get; init; }
        public PlayerDTO PlayerDTO { get; init; }

        public AddPlayerCommand(PlayerDTO playerDTO, ClaimsPrincipal? user = null) {
            PlayerDTO = playerDTO;
            User = user;
        }
    }
}
