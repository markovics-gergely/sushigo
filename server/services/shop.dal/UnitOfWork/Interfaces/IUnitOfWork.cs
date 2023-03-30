using shop.dal.Domain;
using shop.dal.Repository.Interfaces;

namespace shop.dal.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Card> CardRepository { get; }
        IGenericRepository<Deck> DeckRepository { get; }

        Task Save();
    }
}
