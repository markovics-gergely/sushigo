using user.bll.Infrastructure.ViewModels;

namespace user.api.Hubs.Interfaces
{
    /// <summary>
    /// User related events to send
    /// </summary>
    public interface IUserEventsHub
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task RefreshUser(UserViewModel user);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task RemoveGame();
    }
}
