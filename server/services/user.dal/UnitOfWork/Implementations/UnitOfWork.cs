using shared.dal.Repository.Interfaces;
using user.dal.Domain;
using user.dal.UnitOfWork.Interfaces;

namespace user.dal.UnitOfWork.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserDbContext context;
        private readonly IGenericRepository<ApplicationUser> userRepository;
        private readonly IGenericRepository<Friend> friendRepository;
        private readonly IGenericRepository<History> historyRepository;

        public IGenericRepository<ApplicationUser> UserRepository => userRepository;
        public IGenericRepository<Friend> FriendRepository => friendRepository;
        public IGenericRepository<History> HistoryRepository => historyRepository;

        public UnitOfWork(UserDbContext context,
            IGenericRepository<ApplicationUser> userRepository,
            IGenericRepository<Friend> friendRepository,
            IGenericRepository<History> historyRepository)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.friendRepository = friendRepository;
            this.historyRepository = historyRepository;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
