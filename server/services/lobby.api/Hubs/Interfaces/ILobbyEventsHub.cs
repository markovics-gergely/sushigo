using lobby.bll.Infrastructure.ViewModels;

namespace lobby.api.Hubs.Interfaces
{
    /// <summary>
    /// Lobby events hub interface
    /// </summary>
    public interface ILobbyEventsHub
    {
        /// <summary>
        /// Event for adding a player to the lobby
        /// </summary>
        /// <param name="player"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddPlayer(PlayerViewModel player, CancellationToken cancellationToken);

        /// <summary>
        /// Event for removing a player from the lobby
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemovePlayer(Guid playerId, CancellationToken cancellationToken);

        /// <summary>
        /// Event for setting a player ready status in the lobby
        /// </summary>
        /// <param name="lobbyViewModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task PlayerReady(LobbyViewModel lobbyViewModel, CancellationToken cancellationToken);

        /// <summary>
        /// Event for removing the lobby
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Removelobby(CancellationToken cancellationToken);

        /// <summary>
        /// Event for adding a message to the lobby chat
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddMessage(MessageViewModel message, CancellationToken cancellationToken);

        /// <summary>
        /// Event for updating the deck type in the lobby
        /// </summary>
        /// <param name="lobbyViewModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateDeckType(LobbyViewModel lobbyViewModel, CancellationToken cancellationToken);
    }
}
