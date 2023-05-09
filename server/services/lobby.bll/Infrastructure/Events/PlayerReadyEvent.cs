using lobby.bll.Infrastructure.ViewModels;
using MediatR;

namespace lobby.bll.Infrastructure.Events
{
    public class PlayerReadyEvent : INotification
    {
        public LobbyViewModel LobbyViewModel { get; init; }

        public PlayerReadyEvent(LobbyViewModel lobbyViewModel)
        {
            LobbyViewModel = lobbyViewModel;
        }
    }
}
