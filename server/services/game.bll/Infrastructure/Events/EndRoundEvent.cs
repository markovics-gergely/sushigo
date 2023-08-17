using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Events
{
    public class EndRoundEvent : INotification
    {
        public ClaimsPrincipal Principal { get; init; } = null!;
        public Guid GameId { get; init; }
    }
}
