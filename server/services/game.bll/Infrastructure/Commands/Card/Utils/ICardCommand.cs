using game.bll.Infrastructure.DataTransferObjects;
using game.dal.Domain;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card.Utils
{
    public interface ICardCommand<out TCard> where TCard : CardTypeWrapper
    {
        public ClaimsPrincipal? User { get; set; }
        public Task OnEndRound(BoardCard boardCard);
        public Task OnEndTurn(Player player, HandCard handCard);
        public Task OnAfterTurn(Player player, PlayAfterTurnDTO playAfterTurnDTO) { return Task.CompletedTask; }
        public Task OnEndGame(BoardCard boardCard) { return Task.CompletedTask; }
    }

    public static class CardExtensions
    {
        public static ICardCommand<TCard>? GetCommand<TCard>(this IServiceProvider serviceProvider, TCard card) where TCard : CardTypeWrapper
        {
            try
            {
                return (ICardCommand<TCard>?)serviceProvider.GetService(typeof(ICardCommand<>).MakeGenericType(card.GetType()));
            }
            catch { return null; }
        }
    }
}
