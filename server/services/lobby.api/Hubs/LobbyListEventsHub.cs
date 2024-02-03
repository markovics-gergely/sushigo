using lobby.api.Hubs.Interfaces;
using shared.bll.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace lobby.api.Hubs
{
    /// <summary>
    /// Hub center for lobby list events
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LobbyListEventsHub : Hub<ILobbyListEventsHub>
    {
        /// <summary>
        /// Add user to their own group on connection
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
