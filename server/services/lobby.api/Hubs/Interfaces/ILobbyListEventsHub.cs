using lobby.bll.Infrastructure.ViewModels;

namespace lobby.api.Hubs.Interfaces
{
    /// <summary>
    /// Lobby list events hub interface
    /// </summary>
    public interface ILobbyListEventsHub
    {
        /// <summary>
        /// Event for adding a lobby to the lobby list
        /// </summary>
        /// <param name="lobbyItemViewModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddLobby(LobbyItemViewModel lobbyItemViewModel, CancellationToken cancellationToken);

        /// <summary>
        /// Event for removing a lobby from the lobby list
        /// </summary>
        /// <param name="lobbyId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoveLobby(Guid lobbyId, CancellationToken cancellationToken);
    }
}
