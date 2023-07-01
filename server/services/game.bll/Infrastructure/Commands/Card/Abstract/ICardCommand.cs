using game.bll.Infrastructure.DataTransferObjects;
using game.dal.Domain;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card.Abstract
{
    public interface ICardCommand<out TCard> where TCard : CardTypeWrapper
    {
        public ClaimsPrincipal? User { get; set; }
        public Task OnEndRound(BoardCard boardCard);
        public Task OnEndTurn(Player player, PlayCardDTO playCardDTO);
        public Task OnAfterTurn(Player player, PlayAfterTurnDTO playAfterTurnDTO) { return Task.CompletedTask; }
    }
}
