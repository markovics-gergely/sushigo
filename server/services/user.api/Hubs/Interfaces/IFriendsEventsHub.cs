using user.bll.Infrastructure.ViewModels;

namespace user.api.Hubs.Interfaces
{
    /// <summary>
    /// Friend related events to send
    /// </summary>
    public interface IFriendsEventsHub
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="friendStatuses"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task FriendStatuses(List<FriendStatusViewModel> friendStatuses, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="friendStatus"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task FriendStatus(FriendStatusViewModel friendStatus, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task FriendRequest(UserNameViewModel sender, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task FriendRemove(UserNameViewModel sender, CancellationToken cancellationToken);
    }
}
