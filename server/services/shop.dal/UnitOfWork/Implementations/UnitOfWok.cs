using shop.dal.Domain;
using shop.dal.Repository.Interfaces;
using shop.dal.UnitOfWork.Interfaces;

namespace shop.dal.UnitOfWork.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserDbContext context;
        private readonly IGenericRepository<Card> cardRepository;
        private readonly IGenericRepository<Deck> deckRepository;

        public IGenericRepository<Card> CardRepository => cardRepository;
        public IGenericRepository<Deck> DeckRepository => deckRepository;

        public UnitOfWork(UserDbContext context,
            IGenericRepository<Card> cardRepository,
            IGenericRepository<Deck> deckRepository)
        {
            this.context = context;
            this.cardRepository = cardRepository;
            this.deckRepository = deckRepository;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
