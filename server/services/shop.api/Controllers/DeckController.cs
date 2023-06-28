using IdentityServer4.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.dal.Models;
using shop.bll.Infrastructure.Commands;
using shop.bll.Infrastructure.DataTransferObjects;
using shop.bll.Infrastructure.Queries;
using shop.bll.Infrastructure.ViewModels;

namespace shop.api.Controllers
{
    /// <summary>
    /// Deck related endpoints
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Authorize(Roles = RoleTypes.Classic)]
    public class DeckController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="mediator"></param>
        public DeckController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get list of decks
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>List of decks</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeckViewModel>>> GetDecksAsync(CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new GetDecksQuery(user);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Get a deck
        /// </summary>
        /// <param name="deckType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{deckType}")]
        public async Task<ActionResult<IEnumerable<DeckViewModel>>> GetDeckAsync([FromRoute] DeckType deckType, CancellationToken cancellationToken)
        {
            var query = new GetDeckQuery(deckType);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Claim Party role for the user logged in
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("party")]
        public async Task<ActionResult> ClaimParty(CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new BuyPartyCommand(user);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Claim a deck for the user logged in
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("deck")]
        public async Task<ActionResult> ClaimDeck(BuyDeckDTO dto, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new BuyDeckCommand(dto, user);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

    }
}
