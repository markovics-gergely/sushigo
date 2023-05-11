using lobby.api.Hubs.Interfaces;
using lobby.bll.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace lobby.api.Hubs
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LobbyEventsHub : Hub<ILobbyEventsHub>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="mediator"></param>
        public LobbyEventsHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            var group = Context.User?.GetUserLobbyFromJwt() ?? Context.GetHttpContext()?.Request.Query["lobby"].SingleOrDefault();
            if (group != null)
            {
                string id = Context.User?.GetUserIdFromJwt() ?? "";
                Connections.LobbyConnections.Add(id, Context.ConnectionId);
                Groups.AddToGroupAsync(Context.ConnectionId, group);
            }
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string id = Context.User?.GetUserIdFromJwt() ?? "";
            Connections.LobbyConnections.Remove(id, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
