using MediatR;

namespace game.bll.Infrastructure.Events
{
    public class RefreshGameByIdEvent : INotification
    {
        public Guid GameId { get; init; }
    }
}
