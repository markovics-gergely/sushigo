using game.bll.Infrastructure.ViewModels;
using MediatR;

namespace game.bll.Infrastructure.Events
{
    public class RefreshGameEvent : INotification
    {
        public GameViewModel GameViewModel { get; init; } = null!;
    }
}
