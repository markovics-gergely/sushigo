using MediatR;

namespace lobby.bll.Infrastructure.Events
{
    public class RemovePlayerEvent : INotification
    {
        public Guid LobbyId { get; init; }
        public Guid PlayerId { get; init; }
        public RemovePlayerEvent(Guid lobbyId, Guid playerId)
        {
            LobbyId = lobbyId;
            PlayerId = playerId;
        }
    }
}
