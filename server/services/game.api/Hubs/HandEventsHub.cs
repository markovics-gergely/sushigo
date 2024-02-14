using game.api.Hubs.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using shared.bll.Extensions;

namespace game.api.Hubs
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HandEventsHub : Hub<IHandEventsHub>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            var group = Context.User?.GetPlayerIdFromJwt().ToString() ?? Context.GetHttpContext()?.Request.Query["player"].SingleOrDefault();
            if (group != null)
            {
                Groups.AddToGroupAsync(Context.ConnectionId, group);
            }
            return base.OnConnectedAsync();
        }
    }
}
