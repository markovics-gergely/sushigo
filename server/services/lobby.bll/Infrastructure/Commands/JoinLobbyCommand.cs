using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using System.Security.Claims;

namespace lobby.bll.Infrastructure.Commands
{
    public class JoinLobbyCommand : IRequest<LobbyViewModel>
    {
        public ClaimsPrincipal? User { get; init; }
        public JoinLobbyDTO JoinLobbyDTO { get; init; }

        public JoinLobbyCommand(JoinLobbyDTO joinLobbyDTO, ClaimsPrincipal? user = null)
        {
            JoinLobbyDTO = joinLobbyDTO;
            User = user;
        }
    }
}
