using lobby.api.Hubs.Interfaces;
using shared.bll.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace lobby.api.Hubs
{
    /// <summary>
    /// Hub center for lobby events
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LobbyEventsHub : Hub<ILobbyEventsHub>
    {
        /// <summary>
        /// Add user to their lobby group on connection
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            var group = Context.User?.GetUserLobbyFromJwt() ?? Context.GetHttpContext()?.Request.Query["lobby"].SingleOrDefault();
            if (group != null)
            {
                Groups.AddToGroupAsync(Context.ConnectionId, group);
            }
            return base.OnConnectedAsync();
        }
    }
}
