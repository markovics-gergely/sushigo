using game.bll.Infrastructure.ViewModels;
using MediatR;
using System.Security.Claims;

namespace game.bll.Infrastructure.Queries
{
    public class GetOwnHandQuery : IRequest<HandViewModel>
    {
        public ClaimsPrincipal? User { get; set; }
        public GetOwnHandQuery(ClaimsPrincipal? user = null)
        {
            User = user;
        }
    }
}
