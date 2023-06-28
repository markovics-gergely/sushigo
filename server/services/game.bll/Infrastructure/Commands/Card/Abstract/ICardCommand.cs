using game.bll.Infrastructure.DataTransferObjects;
using game.dal.Domain;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card.Abstract
{
    public interface ICardCommand<out TCard> where TCard : CardTypeWrapper
    {
        public ClaimsPrincipal? User { get; set; }
        public Task CalculateEndRound(BoardCard boardCard);
        public Task OnPlay(Player player, PlayCardDTO playCardDTO);
    }
}
