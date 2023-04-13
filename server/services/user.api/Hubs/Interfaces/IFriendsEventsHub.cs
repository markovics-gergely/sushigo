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
        /// <returns></returns>
        Task FriendStatuses(List<FriendStatusViewModel> friendStatuses);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="friendStatus"></param>
        /// <returns></returns>
        Task FriendStatus(FriendStatusViewModel friendStatus);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        Task FriendRequest(UserNameViewModel sender);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        Task FriendRemove(UserNameViewModel sender);
    }
}
