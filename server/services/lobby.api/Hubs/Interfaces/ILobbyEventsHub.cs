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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lobbyViewModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task PlayerReady(LobbyViewModel lobbyViewModel, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Removelobby(CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddMessage(MessageViewModel message, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lobbyViewModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateDeckType(LobbyViewModel lobbyViewModel, CancellationToken cancellationToken);
    }
}
