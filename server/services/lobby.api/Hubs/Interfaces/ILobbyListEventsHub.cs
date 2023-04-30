using lobby.bll.Infrastructure.ViewModels;

namespace lobby.api.Hubs.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILobbyListEventsHub
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lobbyItemViewModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddLobby(LobbyItemViewModel lobbyItemViewModel, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lobbyId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoveLobby(Guid lobbyId, CancellationToken cancellationToken);
    }
}
