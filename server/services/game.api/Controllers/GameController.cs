﻿using game.bll.Infrastructure.Commands;
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
    public class GameController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="serviceProvider"></param>
        public GameController(IMediator mediator, IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Get game
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<GameViewModel>> GetGameAsync(CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new GetGameQuery(user);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Create game
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult<GameViewModel>> CreateGameAsync([FromBody] CreateGameDTO dto, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new CreateGameCommand(dto, user);
            return Ok(await _mediator.Send(command, cancellationToken));
        }

        /// <summary>
        /// Create game
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("end-turn")]
        public async Task<ActionResult<GameViewModel>> EndTurnAsync(CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new ProceedEndTurnCommand(user);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Create game
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("end-round")]
        public async Task<ActionResult<GameViewModel>> EndRoundAsync(CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new ProceedEndRoundCommand(user);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Delete game
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<GameViewModel>> RemoveGameAsync(CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new RemoveGameCommand(user);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}