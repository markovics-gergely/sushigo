using lobby.bll.Infrastructure.ViewModels;
using MediatR;

namespace lobby.bll.Infrastructure.Events
{
    public class UpdateDeckTypeEvent : INotification
    {
        public LobbyViewModel LobbyViewModel { get; init; }

        public UpdateDeckTypeEvent(LobbyViewModel lobbyViewModel)
        {
            LobbyViewModel = lobbyViewModel;
        }
    }
}
