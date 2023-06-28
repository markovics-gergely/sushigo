using shared.dal.Models;

namespace game.bll.Infrastructure.Commands.Deck.Abstract
{
    public interface IDeckCommand<out TDeck> where TDeck : DeckTypeWrapper
    {
        public Task Shuffle();
    }
}
