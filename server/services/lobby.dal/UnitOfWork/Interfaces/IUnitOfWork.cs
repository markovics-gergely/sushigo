using lobby.dal.Domain;
using shared.dal.Repository.Interfaces;

namespace lobby.dal.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Lobby> LobbyRepository { get; }
        IGenericRepository<Player> PlayerRepository { get; }
        IGenericRepository<Message> MessageRepository { get; }

        Task Save();
    }
}
