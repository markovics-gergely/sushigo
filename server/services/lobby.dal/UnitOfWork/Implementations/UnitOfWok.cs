using lobby.dal.Domain;
using lobby.dal.UnitOfWork.Interfaces;
using shared.dal.Repository.Interfaces;

namespace lobby.dal.UnitOfWork.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LobbyDbContext context;
        private readonly IGenericRepository<Lobby> lobbyRepository;
        private readonly IGenericRepository<Player> playerRepository;
        private readonly IGenericRepository<Message> messageRepository;

        public IGenericRepository<Lobby> LobbyRepository => lobbyRepository;
        public IGenericRepository<Player> PlayerRepository => playerRepository;
        public IGenericRepository<Message> MessageRepository => messageRepository;

        public UnitOfWork(LobbyDbContext context,
            IGenericRepository<Lobby> lobbyRepository,
            IGenericRepository<Player> playerRepository,
            IGenericRepository<Message> messageRepository)
        {
            this.context = context;
            this.lobbyRepository = lobbyRepository;
            this.playerRepository = playerRepository;
            this.messageRepository = messageRepository;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
