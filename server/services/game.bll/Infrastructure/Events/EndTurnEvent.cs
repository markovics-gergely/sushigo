using MediatR;

namespace game.bll.Infrastructure.Events
{
    public class EndTurnEvent : INotification
    {
        public Guid GameId { get; init; }
    }
}
