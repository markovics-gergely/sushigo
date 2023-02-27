using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using user.api.Extensions;
using user.api.Hubs.Interfaces;
using user.bll.Extensions;
using user.bll.Infrastructure.Queries;
using user.bll.Infrastructure.ViewModels;

namespace user.api.Hubs
{
    /// <summary>
    /// Hub for friends related events
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FriendEventsHub : Hub<IFriendsEventsHub>
    {
        private readonly IMediator _mediator;
        private readonly ConnectionMapping<string> connectionMapping = new();

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
        public override async Task<Task> OnConnectedAsync()
        {
            string id = Context.User?.GetUserIdFromJwt() ?? "";
            await Groups.AddToGroupAsync(Context.ConnectionId, id);
            connectionMapping.Add(id, Context.ConnectionId);

            await SendStatusToFriends(id, true);
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Event on user disconnect
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            string id = Context.User?.GetUserIdFromJwt() ?? "";
            connectionMapping.Remove(id, Context.ConnectionId);

            await SendStatusToFriends(id, false);
            return base.OnDisconnectedAsync(exception);
        }

        private async Task SendStatusToFriends(string id, bool status)
        {
            var query = new GetFriendsQuery(Context.User);
            var result = await _mediator.Send(query);
            var friends = result.Friends.Concat(result.Sent).Concat(result.Received).Select(friend => friend.Id.ToString());
            await Clients.Groups(friends).FriendStatus(new FriendStatusViewModel { Id = id, Status = status });
        }

        /// <summary>
        /// Send list of friend statuses
        /// </summary>
        /// <param name="guids"></param>
        /// <returns></returns>
        public void LoadFriendStatus(List<string> guids)
        {
            var list = guids.Select(x => new FriendStatusViewModel { Id = x, Status = connectionMapping.Exists(x) }).ToList();
            Clients.Caller.FriendStatuses(list).Wait();
        }
    }
}
