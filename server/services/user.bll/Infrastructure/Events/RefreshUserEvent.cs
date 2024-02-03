using MediatR;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Events
{
    public class RefreshUserEvent : INotification
    {
        public required Guid UserId { get; set; }
        public required UserViewModel User { get; set; }
    }
}
