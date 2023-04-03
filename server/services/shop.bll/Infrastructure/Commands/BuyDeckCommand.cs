using MediatR;
using shop.bll.Infrastructure.DataTransferObjects;
using System.Security.Claims;

namespace shop.bll.Infrastructure.Commands
{
    public class BuyDeckCommand : IRequest
    {
        public ClaimsPrincipal? User { get; }
        public BuyDeckDTO BuyDeckDTO { get; }
        public BuyDeckCommand(BuyDeckDTO buyDeckDTO, ClaimsPrincipal? user = null)
        {
            BuyDeckDTO = buyDeckDTO;
            User = user;
        }
    }
}
