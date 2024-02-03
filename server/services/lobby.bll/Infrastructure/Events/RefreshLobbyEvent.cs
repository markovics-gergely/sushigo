using lobby.bll.Infrastructure.ViewModels;
using MediatR;

namespace lobby.bll.Infrastructure.Events
{
    public class RefreshLobbyEvent : INotification
    {
        public LobbyViewModel Lobby { get; init; }
        public RefreshLobbyEvent(LobbyViewModel lobby)
        {
            Lobby = lobby;
        }
    }
}
