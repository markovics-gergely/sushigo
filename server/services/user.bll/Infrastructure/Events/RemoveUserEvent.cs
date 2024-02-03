using MediatR;

namespace user.bll.Infrastructure.Events
{
    public class RemoveUserEvent : INotification
    {
        public required Guid UserId { get; set; }
    }
}
