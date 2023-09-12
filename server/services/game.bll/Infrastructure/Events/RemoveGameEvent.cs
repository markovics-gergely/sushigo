using MediatR;

namespace game.bll.Infrastructure.Events
{
    public class RemoveGameEvent : INotification
    {
        public Guid GameId { get; init; }
    }
}
