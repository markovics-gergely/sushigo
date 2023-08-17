using game.bll.Infrastructure.ViewModels;

namespace game.api.Hubs.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGameEventsHub
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameViewModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RefreshGame(GameViewModel gameViewModel, CancellationToken cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task EndTurn(CancellationToken cancellationToken);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task EndRound(CancellationToken cancellationToken);
    }
}
