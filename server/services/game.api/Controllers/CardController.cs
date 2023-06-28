using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.Commands.Card.Abstract;
using IdentityServer4.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
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
        public CardController(IMediator mediator, IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Get a lobby by id
        /// </summary>
        /// <param name="id">Id of the lobby</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("{card}")]
        public async Task<ActionResult> GetLobbyAsync([FromRoute] CardType card, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new PlayCardCommand(card, user);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}
