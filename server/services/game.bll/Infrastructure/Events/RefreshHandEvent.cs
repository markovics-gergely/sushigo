using game.bll.Infrastructure.ViewModels;
using MediatR;

namespace game.bll.Infrastructure.Events
{
    public class RefreshHandEvent : INotification
    {
        public Guid PlayerId { get; init; }
        public HandViewModel Hand { get; set; } = new HandViewModel();
    }
}
