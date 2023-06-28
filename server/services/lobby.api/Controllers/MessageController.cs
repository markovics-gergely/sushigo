using IdentityServer4.Extensions;
using lobby.bll.Infrastructure.Commands;
using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.Queries;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.dal.Models;

namespace lobby.api.Controllers
{
    /// <summary>
    /// Route for message endpoints
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Authorize(Roles = RoleTypes.Classic)]
    public class MessageController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="mediator"></param>
        public MessageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Ge messages for lobby
        /// </summary>
        /// <param name="lobbyId">Lobby of the messages</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{lobbyId}")]
        public async Task<ActionResult<IEnumerable<MessageViewModel>>> GetMessagesAsync([FromRoute] Guid lobbyId, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new GetMessagesQuery(lobbyId, user);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Add a message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MessageViewModel>> AddMessageAsync([FromBody] MessageDTO message, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new CreateMessageCommand(message, user);
            return Ok(await _mediator.Send(command, cancellationToken));
        }
    }
}
