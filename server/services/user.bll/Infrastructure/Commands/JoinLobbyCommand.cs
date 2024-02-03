using MediatR;
using shared.dal.Models;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Commands
{
    public class JoinLobbyCommand : IRequest<UserViewModel>
    {
        public LobbyJoinedDTO LobbyJoinedDTO { get; }

        public JoinLobbyCommand(LobbyJoinedDTO lobbyJoinedDTO)
        {
            LobbyJoinedDTO = lobbyJoinedDTO;
        }
    }
}
