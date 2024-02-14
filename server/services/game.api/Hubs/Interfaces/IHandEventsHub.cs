using game.bll.Infrastructure.ViewModels;

namespace game.api.Hubs.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHandEventsHub
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handViewModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RefreshHand(HandViewModel handViewModel, CancellationToken cancellationToken);
    }
}
