using game.bll.Infrastructure.Commands.Deck.Abstract;
using shared.dal.Models;

namespace game.bll.Infrastructure.Commands.Deck
{
    internal class MyFirstMealCommand : IDeckCommand<MyFirstMeal>
    {
        public Task Shuffle()
        {
            throw new NotImplementedException();
        }
    }
}
