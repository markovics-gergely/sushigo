using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using shared.Models;
using System.Security.Claims;
using user.dal.Domain;
using user.dal.Types;

namespace user.api.Extensions
{
    /// <summary>
    /// Manage claims for identityserver
    /// </summary>
    public class ClaimsFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Add dependencies to factory
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="optionsAccessor"></param>
        public ClaimsFactory(
            UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
            _userManager = userManager;
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
            if (user.Experience >= RoleTypes.PartyExp)
            {
                identity.AddClaim(new Claim(JwtClaimTypes.Role, "CanClaimParty"));
            }
            identity.AddClaim(new Claim(RoleTypes.ExpClaim, user.Experience.ToString()));
            identity.AddClaims(user.DeckClaims.Select(g => new Claim(RoleTypes.GameClaim, g.ToString())));

            return identity;
        }
    }
}
