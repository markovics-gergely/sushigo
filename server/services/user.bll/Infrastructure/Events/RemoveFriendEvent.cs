using MediatR;

namespace user.bll.Infrastructure.Events
{
    public class RemoveFriendEvent : INotification
    {
        public Guid ReceiverId { get; set; }
        public Guid SenderId { get; set; }
    }
}
