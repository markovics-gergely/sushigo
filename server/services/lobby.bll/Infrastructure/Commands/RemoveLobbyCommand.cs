using lobby.bll.Infrastructure.ViewModels;
using MediatR;

namespace lobby.bll.Infrastructure.Commands
{
    public class RemoveLobbyCommand : IRequest<LobbyViewModel?>
    {
        public Guid LobbyId { get; init; } = Guid.Empty;
        public RemoveLobbyCommand(Guid lobbyId) {
            LobbyId = lobbyId;
        }
    }
}
