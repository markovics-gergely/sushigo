using lobby.bll.Infrastructure.ViewModels;
using MediatR;

namespace lobby.bll.Infrastructure.Events
{
    public class AddLobbyEvent : INotification
    {
        public LobbyItemViewModel Lobby { get; init; }
        public AddLobbyEvent(LobbyItemViewModel lobby)
        {
            Lobby = lobby;
        }
    }
}
