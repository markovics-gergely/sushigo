using shared.dal.Repository.Interfaces;
using shop.dal.Domain;

namespace shop.dal.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Card> CardRepository { get; }
        IGenericRepository<Deck> DeckRepository { get; }

        Task Save();
    }
}
