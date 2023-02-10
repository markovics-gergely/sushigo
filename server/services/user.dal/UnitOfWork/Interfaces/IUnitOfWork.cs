using user.dal.Domain;
using user.dal.Repository.Interfaces;

namespace user.dal.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<ApplicationUser> UserRepository { get; }
        IGenericRepository<Friend> FriendRepository { get; }

        Task Save();
    }
}
