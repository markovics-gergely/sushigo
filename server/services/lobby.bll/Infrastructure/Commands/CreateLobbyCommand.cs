using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using System.Security.Claims;

namespace lobby.bll.Infrastructure.Commands
{
    public class CreateLobbyCommand : IRequest<LobbyViewModel>
    {
        public ClaimsPrincipal? User { get; set; }
        public LobbyDTO Lobby { get; private set; }

        public CreateLobbyCommand(LobbyDTO lobby, ClaimsPrincipal? user = null)
        {
            Lobby = lobby;
            User = user;
        }
    }
}
