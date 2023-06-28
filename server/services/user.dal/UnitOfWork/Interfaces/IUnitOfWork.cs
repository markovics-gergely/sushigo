using shared.dal.Repository.Interfaces;
using user.dal.Domain;

namespace user.dal.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<ApplicationUser> UserRepository { get; }
        IGenericRepository<Friend> FriendRepository { get; }

        Task Save();
    }
}
