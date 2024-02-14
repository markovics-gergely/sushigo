using game.bll.Infrastructure.DataTransferObjects;
using game.dal.Domain;
using shared.dal.Models;

namespace game.bll.Infrastructure.Commands.Card.Utils
{
    public interface ICardCommand<out TCard> where TCard : CardTypeWrapper
    {
        /// <summary>
        /// Calculate end of turn action of board card
        /// </summary>
        /// <param name="boardCard"></param>
        /// <returns>List of calculated board card ids</returns>
        public Task<List<Guid>> OnEndRound(BoardCard boardCard);
        public Task OnEndTurn(Player player, HandCard handCard);
        public Task OnAfterTurn(Player player, PlayAfterTurnDTO playAfterTurnDTO) { return Task.CompletedTask; }
        /// <summary>
        /// Calculate end of game action of board card
        /// </summary>
        /// <param name="boardCard"></param>
        /// <returns>List of calculated board card ids</returns>
        public Task<List<Guid>> OnEndGame(BoardCard boardCard) { return Task.FromResult(new List<Guid>()); }
        public Task OnPlayCard(HandCard handCard) { return Task.CompletedTask; }
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
