using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.DataTransferObjects;
using game.bll.Infrastructure.Queries;
using game.bll.Infrastructure.ViewModels;
using IdentityServer4.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.dal.Models;

namespace game.api.Controllers
{
    /// <summary>
    /// Route for lobby endpoints
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Authorize(Roles = RoleTypes.Classic)]
    public class CardController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="serviceProvider"></param>
        public CardController(IMediator mediator, IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Get hand
        /// </summary>
        /// <param name="handId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("hand/{handId}")]
        public async Task<ActionResult<HandViewModel>> GetHandAsync([FromRoute] Guid handId, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new GetHandQuery(handId, user);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Play a card
        /// </summary>
        /// <param name="playCardDTO"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PlayCardAsync([FromBody] PlayCardDTO playCardDTO, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new PlayCardCommand(playCardDTO, user);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Play a card after the turn
        /// </summary>
        /// <param name="playAfterTurnDTO"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("after-turn")]
        public async Task<ActionResult> PlayAfterTurnAsync([FromBody] PlayAfterTurnDTO playAfterTurnDTO, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new PlayAfterTurnCommand(playAfterTurnDTO, user);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Skip playing after the turn
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("skip-after")]
        public async Task<ActionResult> SkipAfterTurnAsync(CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new SkipAfterTurnCommand(user);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}
