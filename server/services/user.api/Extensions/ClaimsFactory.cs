using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using shared.dal.Models;
using System.Security.Claims;
using user.bll.Infrastructure.Queries;
using user.dal.Domain;

namespace user.api.Extensions
{
    /// <summary>
    /// Manage claims for identityserver
    /// </summary>
    public class ClaimsFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        /// <summary>
        /// Add dependencies to factory
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="mediator"></param>
        /// <param name="optionsAccessor"></param>
        public ClaimsFactory(
            UserManager<ApplicationUser> userManager,
            IMediator mediator,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        /// <summary>
        /// Generate claim data for identity
        /// </summary>
        /// <param name="user">Data of current user</param>
        /// <returns></returns>
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            identity.AddClaims(roles.Select(role => new Claim(JwtClaimTypes.Role, role)));
            if (user.Experience >= RoleTypes.PartyExp && !roles.Contains("Party"))
            {
                identity.AddClaim(new Claim(JwtClaimTypes.Role, "CanClaimParty"));
            }
            if (user.Experience >= RoleTypes.DeckExp && roles.Contains("Party"))
            {
                identity.AddClaim(new Claim(JwtClaimTypes.Role, "CanClaimDeck"));
            }
            identity.AddClaim(new Claim(RoleTypes.ExpClaim, user.Experience.ToString()));
            identity.AddClaims(user.DeckClaims.Select(g => new Claim(RoleTypes.DeckClaim, g.ToString())));
            identity.AddClaim(new Claim(RoleTypes.LobbyClaim, user.ActiveLobby?.ToString() ?? ""));
            identity.AddClaim(new Claim(RoleTypes.GameClaim, user.ActiveGame?.ToString() ?? ""));

            var userVM = await _mediator.Send(new GetUserByIdQuery(user.Id.ToString()));
            identity.AddClaim(new Claim(RoleTypes.AvatarClaim, userVM.Avatar ?? ""));

            return identity;
        }
    }
}
