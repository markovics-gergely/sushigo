using lobby.dal.Domain;
using lobby.dal.Repository.Interfaces;

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
