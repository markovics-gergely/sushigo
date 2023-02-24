using Microsoft.AspNetCore.SignalR;
using user.bll.Extensions;

namespace user.api.Hubs
{
    /// <summary>
    /// Hub for friends related events
    /// </summary>
    public class FriendEventsHub : Hub
    {
        /// <summary>
        /// Event on user connection
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            string id = Context.User?.GetUserIdFromJwt() ?? "";
            Groups.AddToGroupAsync(Context.ConnectionId, id);
            return base.OnConnectedAsync();
        }
    }
}
