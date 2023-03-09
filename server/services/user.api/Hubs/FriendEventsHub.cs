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
        private readonly static ConnectionMapping<string> _connections = new();

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
            _connections.Add(id, Context.ConnectionId);

            var query = new GetFriendsQuery(Context.User);
            var result = await _mediator.Send(query);
            var friends = result?.Friends?.Concat(result.Sent)?.Concat(result.Received)?.Select(friend => friend.Id.ToString()) ?? new List<string>();

            Task.WaitAll(
                SendStatusToFriends(friends, true),
                LoadFriendStatus(friends)
            );
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
            _connections.Remove(id, Context.ConnectionId);

            var query = new GetFriendsQuery(Context.User);
            var result = await _mediator.Send(query);
            var friends = result?.Friends?.Concat(result.Sent)?.Concat(result.Received)?.Select(friend => friend.Id.ToString()) ?? new List<string>();

            Task.WaitAll(
                SendStatusToFriends(friends, false),
                LoadFriendStatus(friends)
            );
            return base.OnDisconnectedAsync(exception);
        }

        private async Task SendStatusToFriends(IEnumerable<string> friends, bool status)
        {
            await Clients.Groups(friends).FriendStatus(new FriendStatusViewModel { Id = Context.User?.GetUserIdFromJwt() ?? "", Status = status });
        }

        /// <summary>
        /// Send list of friend statuses
        /// </summary>
        /// <returns></returns>
        public async Task LoadFriendStatus(IEnumerable<string> friends)
        {
            var list = friends.Select(x => new FriendStatusViewModel { Id = x, Status = _connections.Exists(x) }).ToList();
            await Clients.Caller.FriendStatuses(list);
        }
    }
}
