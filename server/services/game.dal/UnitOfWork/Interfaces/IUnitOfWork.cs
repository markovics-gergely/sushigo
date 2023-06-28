using game.dal.Domain;
using shared.dal.Repository.Interfaces;

namespace game.dal.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Board> BoardRepository { get; }
        IGenericRepository<BoardCard> BoardCardRepository { get; }
        IGenericRepository<Deck> DeckRepository { get; }
        IGenericRepository<Game> GameRepository { get; }
        IGenericRepository<Hand> HandRepository { get; }
        IGenericRepository<HandCard> HandCardRepository { get; }
        IGenericRepository<Player> PlayerRepository { get; }

        Task Save();
    }
}
