using MediatR;

namespace game.bll.Infrastructure.Events
{
    public class EndRoundEvent : INotification
    {
        public Guid GameId { get; init; }
    }
}
