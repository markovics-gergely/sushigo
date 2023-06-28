using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.DataTransferObjects;
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
    public class GameController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="mediator"></param>
        public GameController(IMediator mediator, IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Create game
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult<GameViewModel>> CreateGameAsync([FromBody] CreateGameDTO dto, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new CreateGameCommand(dto, user);
            return Ok(await _mediator.Send(command, cancellationToken));
        }
    }
}
