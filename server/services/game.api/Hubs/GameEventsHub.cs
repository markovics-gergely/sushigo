using game.api.Hubs.Interfaces;
using MassTransit.Mediator;
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
    public class GameEventsHub : Hub<IGameEventsHub>
    {
        private readonly IMediator _mediator;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public GameEventsHub(IMediator mediator) {
            _mediator = mediator;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            var group = Context.User?.GetGameIdFromJwt().ToString() ?? Context.GetHttpContext()?.Request.Query["game"].SingleOrDefault();
            if (group != null)
            {
                Groups.AddToGroupAsync(Context.ConnectionId, group);
            }
            return base.OnConnectedAsync();
        }
    }
}
