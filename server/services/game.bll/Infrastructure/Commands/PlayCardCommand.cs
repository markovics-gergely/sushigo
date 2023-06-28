using MediatR;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands
{
    public class PlayCardCommand : IRequest
    {
        public ClaimsPrincipal? User { get; }
        public CardType CardType { get; }
        public PlayCardCommand(CardType cardType, ClaimsPrincipal? user = null) {
            CardType = cardType;
            User = user;
        }
    }
}
