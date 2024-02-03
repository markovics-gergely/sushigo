using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using System.Security.Claims;

namespace lobby.bll.Infrastructure.Commands
{
    public class UpdateLobbyDeckCommand : IRequest<LobbyViewModel>
    {
        public ClaimsPrincipal? User { get; private set; }
        public UpdateLobbyDTO UpdateLobbyDTO { get; private set; }

        public UpdateLobbyDeckCommand(UpdateLobbyDTO updateLobbyDTO, ClaimsPrincipal? user = null)
        {
            User = user;
            UpdateLobbyDTO = updateLobbyDTO;
        }
    }
}
