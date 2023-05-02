using IdentityServer4.Extensions;
using lobby.bll.Infrastructure.Commands;
using lobby.bll.Infrastructure.DataTransferObjects;
using lobby.bll.Infrastructure.Queries;
using lobby.bll.Infrastructure.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.Models;
using System.Runtime.CompilerServices;

namespace lobby.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Authorize(Roles = RoleTypes.Classic)]
    public class LobbyController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="mediator"></param>
        public LobbyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get list of lobbies
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LobbyItemViewModel>>> GetLobbiesAsync(CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new GetLobbiesQuery();
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Get a lobby by id
        /// </summary>
        /// <param name="id">Id of the lobby</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<LobbyViewModel>> GetLobbyAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new GetLobbyQuery(id, user);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Create a lobby
        /// </summary>
        /// <param name="lobby">Lobby to create</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<LobbyViewModel>> CreateLobbyAsync(LobbyDTO lobby, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new CreateLobbyCommand(lobby, user);
            return Ok(await _mediator.Send(command, cancellationToken));
        }

        /// <summary>
        /// Update deck associated with the lobby
        /// </summary>
        /// <param name="lobby">Lobby data to update with</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("deck")]
        public async Task<ActionResult<LobbyViewModel>> UpdateLobbyDeckAsync([FromBody] UpdateLobbyDTO lobby, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new UpdateLobbyDeckCommand(lobby, user);
            return Ok(await _mediator.Send(command, cancellationToken));
        }

        [HttpPost("player")]
        public async Task<ActionResult<LobbyViewModel>> AddPlayerAsync(PlayerDTO playerDTO, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new AddPlayerCommand(playerDTO);
            return Ok(_mediator.Send(command, cancellationToken));
        }
    }
}
