using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using static IdentityModel.OidcConstants;
using IdentityServer4.Services;
using IdentityServer4.Events;

namespace user.api.Extensions
{
    /// <summary>
    /// IResourceOwnerPasswordValidator that integrates with ASP.NET Identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    public class ResourceOwnerPasswordValidatorWithEmailAndUsername<TUser> : IResourceOwnerPasswordValidator
        where TUser : class
    {
        private readonly SignInManager<TUser> _signInManager;
        private readonly IEventService _events;
        private readonly UserManager<TUser> _userManager;
        private readonly ILogger<ResourceOwnerPasswordValidatorWithEmailAndUsername<TUser>> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOwnerPasswordValidatorWithEmailAndUsername{TUser}"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="events">The events.</param>
        /// <param name="logger">The logger.</param>
        public ResourceOwnerPasswordValidatorWithEmailAndUsername(
            UserManager<TUser> userManager,
            SignInManager<TUser> signInManager,
            IEventService events,
            ILogger<ResourceOwnerPasswordValidatorWithEmailAndUsername<TUser>> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _events = events;
            _logger = logger;
        }

        /// <summary>
        /// Validates the resource owner password credential
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.FindByNameAsync(context.UserName);
            user ??= await _userManager.FindByEmailAsync(context.UserName);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, context.Password, true);
                if (result.Succeeded)
                {
                    var sub = await _userManager.GetUserIdAsync(user);

                    _logger.LogInformation("Credentials validated for {username}", context.UserName);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, sub, context.UserName, interactive: false));

                    context.Result = new GrantValidationResult(sub, AuthenticationMethods.Password);
                    return;
                }
                else if (result.IsLockedOut)
                {
                    _logger.LogInformation("Authentication failed for {username}, reason: locked out", context.UserName);
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "locked out", interactive: false));
                }
                else if (result.IsNotAllowed)
                {
                    _logger.LogInformation("Authentication failed for {username}, reason: not allowed", context.UserName);
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "not allowed", interactive: false));
                }
                else
                {
                    _logger.LogInformation("Authentication failed for {username}, reason: invalid credentials", context.UserName);
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid credentials", interactive: false));
                }
            }
            else
            {
                _logger.LogInformation("No user found matching {username}", context.UserName);
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username or e-mail", interactive: false));
            }
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }
    }
}
