using game.bll.Infrastructure.ViewModels;
using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Queries
{
    public class GetHandQuery : IRequest<HandViewModel>
    {
        public ClaimsPrincipal? User { get; set; }
        public Guid HandId { get; set; }
        public GetHandQuery(Guid handId, ClaimsPrincipal? user = null)
        {
            HandId = handId;
            User = user;
        }
    }
}
