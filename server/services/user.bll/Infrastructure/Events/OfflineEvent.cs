using MediatR;

namespace user.bll.Infrastructure.Events
{
    public class OfflineEvent : INotification
    {
        public required string UserId { get; set; }
        public required List<string> Friends { get; set; }
    }
}
