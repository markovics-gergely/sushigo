using IdentityServer4.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.dal.Configurations.Interfaces;
using shared.dal.Models;
using user.bll.Infrastructure.Commands;
using user.bll.Infrastructure.DataTransferObjects;
using user.bll.Infrastructure.Queries;
using user.bll.Infrastructure.ViewModels;

namespace user.api.Controllers
{
    /// <summary>
    /// Controller for users
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Authorize(Roles = RoleTypes.Classic)]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFileConfigurationService _config;

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="config"></param>
        public UserController(IMediator mediator, IFileConfigurationService config)
        {
            _mediator = mediator;
            _config = config;
        }

        /// <summary>
        /// Get config values from the server
        /// </summary>
        /// <returns></returns>
        [HttpGet("config")]
        public IActionResult GetConfig()
        {
            return Ok(new { MaxUploadSize = _config.GetMaxUploadSize() });
        }

        /// <summary>
        /// Get a user by id
        /// </summary>
        /// <param name="userid">Id of the user</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Values of a user</returns>
        [HttpGet("{userid}")]
        public async Task<ActionResult<UserViewModel>> GetUserByIdAsync(string userid, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new GetUserQuery(userid, user);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Get a user data
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Values of the user</returns>
        [HttpGet]
        public async Task<ActionResult<UserViewModel>> GetUserAsync(CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var query = new GetUserQuery(null, user);
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Remove a user
        /// </summary>
        /// <param name="cancellationToken"></param>
        [HttpDelete]
        public async Task<ActionResult> RemoveUserAsync(CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new RemoveUserCommand(user);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Get users with the given role
        /// </summary>
        /// <param name="role">Searched role</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Users with the given role</returns>
        [Authorize(Roles = RoleTypes.Classic)]
        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<UserNameViewModel>>> GetUsersByRoleAsync(string role, CancellationToken cancellationToken)
        {
            var query = new GetUsersByRoleQuery(new List<string> { role });
            return Ok(await _mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Edit actual user data
        /// </summary>
        /// <param name="userDTO">values to edit</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Success</returns>
        [HttpPut("edit")]
        public async Task<ActionResult<UserViewModel>> EditUserAsync([FromForm] EditUserDTO userDTO, CancellationToken cancellationToken)
        {
            var user = HttpContext.User.IsAuthenticated() ? HttpContext.User : null;
            var command = new EditUserCommand(userDTO, user);
            return await _mediator.Send(command, cancellationToken);
        }

        /// <summary>
        /// Register a user
        /// </summary>
        /// <param name="userDTO">values to edit</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Success</returns>
        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<ActionResult<bool>> RegisterUserAsync([FromBody] RegisterUserDTO userDTO, CancellationToken cancellationToken)
        {
            var command = new CreateUserCommand(userDTO);
            return await _mediator.Send(command, cancellationToken);
        }

        /// <summary>
        /// Edit roles of users
        /// </summary>
        /// <param name="userRoleDTO">user to edit</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Success</returns>
        [Authorize(Roles = RoleTypes.Classic)]
        [HttpPost("role/change")]
        public async Task<ActionResult> EditUserRolesAsync([FromBody] EditUserRoleDTO userRoleDTO, CancellationToken cancellationToken)
        {
            var command = new EditUserRoleCommand(userRoleDTO);
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}
