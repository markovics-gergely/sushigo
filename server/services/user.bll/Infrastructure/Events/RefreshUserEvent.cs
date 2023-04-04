using MediatR;

namespace user.bll.Infrastructure.Events
{
    public class RefreshUserEvent : INotification
    {
        public required string UserId { get; set;}
    }
}
