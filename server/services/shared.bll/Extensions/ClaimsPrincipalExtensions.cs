using IdentityModel;
using shared.dal.Models;
using System.Security.Claims;

namespace shared.bll.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserIdFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject)?.Value ?? string.Empty;
        }

        public static string GetUserNameFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                return claimsPrincipal.Claims.First(x => x.Type == JwtClaimTypes.Name).Value;
            }
            catch { return string.Empty; }
        }

        public static int GetUserExpFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                return int.Parse(claimsPrincipal.Claims.First(x => x.Type == RoleTypes.ExpClaim).Value);
            }
            catch { return 0; }
        }

        public static IEnumerable<DeckType> GetUserDecksFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.Where(x => x.Type == RoleTypes.DeckClaim).Select(x => (DeckType)Enum.Parse(typeof(DeckType), x.Value));
        }

        public static string GetUserAvatarFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                return claimsPrincipal.Claims.First(x => x.Type == RoleTypes.AvatarClaim).Value;
            }
            catch { return string.Empty; }
        }

        public static string GetUserLobbyFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                return claimsPrincipal.Claims.First(x => x.Type == RoleTypes.LobbyClaim).Value;
            }
            catch { return string.Empty; }
        }

        public static Guid GetPlayerIdFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                return Guid.Parse(claimsPrincipal.Claims.First(x => x.Type == RoleTypes.PlayerClaim).Value);
            }
            catch { return Guid.Empty; }
        }

        public static Guid GetGameIdFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                return Guid.Parse(claimsPrincipal.Claims.First(x => x.Type == RoleTypes.GameClaim).Value);
            }
            catch { return Guid.Empty; }
        }

        public static bool GetUserHasDeck(this ClaimsPrincipal claimsPrincipal, DeckType deck)
        {
            return claimsPrincipal.GetUserDecksFromJwt().Any(x => x == deck);
        }
    }
}
