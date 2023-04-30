using lobby.bll.Infrastructure.ViewModels;
using MediatR;

namespace lobby.bll.Infrastructure.Events
{
    public class AddMessageEvent : INotification
    {
        public MessageViewModel Message { get; init; }
        public Guid LobbyId { get; init; }
        public AddMessageEvent(MessageViewModel message, Guid lobbyId)
        {
            Message = message;
            LobbyId = lobbyId;
        }
    }
}
