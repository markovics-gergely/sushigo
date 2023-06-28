using lobby.api.Hubs.Interfaces;
using shared.bll.Extensions;
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
    public class LobbyListEventsHub : Hub<ILobbyListEventsHub>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="mediator"></param>
        public LobbyListEventsHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            string id = Context.User?.GetUserIdFromJwt() ?? "";
            Connections.LobbyListConnections.Add(id, Context.ConnectionId);
            Groups.AddToGroupAsync(Context.ConnectionId, id);
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
            Connections.LobbyListConnections.Remove(id, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
