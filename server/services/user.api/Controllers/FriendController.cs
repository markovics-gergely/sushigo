using IdentityServer4.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.dal.Models.Types;
using user.bll.Infrastructure.Commands;
using user.bll.Infrastructure.Queries;
using user.bll.Infrastructure.ViewModels;

namespace user.api.Controllers
{
    /// <summary>
    /// Controller for friends
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Authorize(Roles = RoleTypes.Classic)]
    public class FriendController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="mediator"></param>
        public FriendController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Add friend to user logged in
        /// </summary>
        /// <param name="name">Username of the user to add</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("{name}")]
        public async Task<ActionResult<UserNameViewModel>> AddFriendAsync([FromRoute] string name, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new AddFriendCommand(name, user);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Remove friend from user logged in
        /// </summary>
        /// <param name="id">Id of the user to remove</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveFriendAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new RemoveFriendCommand(id, user);
            await _mediator.Send(query, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Get list of friends
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<FriendListViewModel>> GetFriendsAsync(CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new GetFriendsQuery(user);
            return Ok(await _mediator.Send(query, cancellationToken));
        }
    }
}
