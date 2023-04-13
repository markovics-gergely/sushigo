using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using user.api.Extensions;
using user.api.Hubs.Interfaces;
using user.bll.Extensions;
using user.bll.Infrastructure.Commands;
using user.bll.Infrastructure.ViewModels;

namespace user.api.Hubs
{
    /// <summary>
    /// Hub for friends related events
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FriendEventsHub : Hub<IEventsHub>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="mediator"></param>
        public FriendEventsHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Event on user connection
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            string id = Context.User?.GetUserIdFromJwt() ?? "";
            Groups.AddToGroupAsync(Context.ConnectionId, id);
            Connections.UserConnections.Add(id, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Event on user disconnect
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string id = Context.User?.GetUserIdFromJwt() ?? "";
            Connections.UserConnections.Remove(id, Context.ConnectionId);

            var command = new UpdateFriendOfflineCommand(id);
            _mediator.Send(command);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
