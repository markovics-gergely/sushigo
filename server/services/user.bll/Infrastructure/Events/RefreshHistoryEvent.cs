using MediatR;

namespace user.bll.Infrastructure.Events
{
    public class RefreshHistoryEvent : INotification
    {
        public required string UserId { get; set; }
    }
}
