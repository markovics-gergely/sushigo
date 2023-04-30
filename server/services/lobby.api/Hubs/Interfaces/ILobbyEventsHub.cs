using lobby.bll.Infrastructure.ViewModels;

namespace lobby.api.Hubs.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILobbyEventsHub
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddPlayer(PlayerViewModel player, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemovePlayer(Guid playerId, CancellationToken cancellationToken);
    }
}
