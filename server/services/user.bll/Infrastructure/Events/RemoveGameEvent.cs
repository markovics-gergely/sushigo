using MediatR;

namespace user.bll.Infrastructure.Events
{
    public class RemoveGameEvent : INotification
    {
        public required string UserId { get; set; }
    }
}
