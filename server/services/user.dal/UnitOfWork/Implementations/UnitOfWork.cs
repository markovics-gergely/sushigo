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

        public IGenericRepository<ApplicationUser> UserRepository => userRepository;
        public IGenericRepository<Friend> FriendRepository => friendRepository;

        public UnitOfWork(UserDbContext context,
            IGenericRepository<ApplicationUser> userRepository,
            IGenericRepository<Friend> friendRepository)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.friendRepository = friendRepository;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
