using lobby.bll.Infrastructure.ViewModels;
using MediatR;

namespace lobby.bll.Infrastructure.Events
{
    public class AddPlayerEvent : INotification
    {
        public Guid LobbyId { get; init; }
        public PlayerViewModel Player { get; init; }
        public AddPlayerEvent(Guid lobbyId, PlayerViewModel player)
        {
            LobbyId = lobbyId;
            Player = player;
        }
    }
}
