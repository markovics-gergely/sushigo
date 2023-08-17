using game.dal.Domain;

namespace game.bll.Infrastructure.Commands.Card.Utils
{
    public interface ISimpleAddToBoard
    {
        public Task AddToBoard(Player player, HandCard handCard);
        public Task AddNigiriToBoard(Player player, HandCard handCard);
    }
}
