using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands
{
    public class MenuCardSelectCommand : IRequest
    {
        public ClaimsPrincipal? User { get; set; }
        public Guid HandCardId { get; set; }
        public MenuCardSelectCommand(Guid handCardId, ClaimsPrincipal? user = null)
        {
            User = user;
            HandCardId = handCardId;
        }
    }
}
