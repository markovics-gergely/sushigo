using MediatR;

namespace lobby.bll.Infrastructure.Events
{
    public class RemoveLobbyEvent : INotification
    {
        public Guid LobbyId { get; init; }
        public RemoveLobbyEvent(Guid lobbyId) {
            LobbyId = lobbyId;
        }
    }
}
