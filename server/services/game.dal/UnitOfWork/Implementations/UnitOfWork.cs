using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using shared.dal.Repository.Interfaces;

namespace game.dal.UnitOfWork.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GameDbContext context;
        private readonly IGenericRepository<CardInfo> cardInfoRepository;
        private readonly IGenericRepository<Board> boardRepository;
        private readonly IGenericRepository<BoardCard> boardCardRepository;
        private readonly IGenericRepository<Deck> deckRepository;
        private readonly IGenericRepository<Game> gameRepository;
        private readonly IGenericRepository<Hand> handRepository;
        private readonly IGenericRepository<HandCard> handCardRepository;
        private readonly IGenericRepository<Player> playerRepository;

        public IGenericRepository<CardInfo> CardInfoRepository => cardInfoRepository;
        public IGenericRepository<Board> BoardRepository => boardRepository;
        public IGenericRepository<BoardCard> BoardCardRepository => boardCardRepository;
        public IGenericRepository<Deck> DeckRepository => deckRepository;
        public IGenericRepository<Game> GameRepository => gameRepository;
        public IGenericRepository<Hand> HandRepository => handRepository;
        public IGenericRepository<HandCard> HandCardRepository => handCardRepository;
        public IGenericRepository<Player> PlayerRepository => playerRepository;

        public UnitOfWork(GameDbContext context,
            IGenericRepository<CardInfo> cardInfoRepository,
            IGenericRepository<Board> boardRepository,
            IGenericRepository<BoardCard> boardCardRepository,
            IGenericRepository<Deck> deckRepository,
            IGenericRepository<Game> gameRepository,
            IGenericRepository<Hand> handRepository,
            IGenericRepository<HandCard> handCardRepository,
            IGenericRepository<Player> playerRepository)
        {
            this.context = context;
            this.cardInfoRepository = cardInfoRepository;
            this.boardCardRepository = boardCardRepository;
            this.boardRepository = boardRepository;
            this.deckRepository = deckRepository;
            this.gameRepository = gameRepository;
            this.handRepository = handRepository;
            this.playerRepository = playerRepository;
            this.handCardRepository = handCardRepository;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
        }
    }
}
