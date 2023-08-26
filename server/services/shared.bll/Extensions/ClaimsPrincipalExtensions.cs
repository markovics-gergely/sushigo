using shared.dal.Models;
using System.Security.Claims;

namespace shared.bll.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserIdFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.First(x => x.Type == "sub").Value;
        }

        public static string GetUserNameFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.First(x => x.Type == "name").Value;
        }

        public static int GetUserExpFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            return int.Parse(claimsPrincipal.Claims.First(x => x.Type == RoleTypes.ExpClaim).Value);
        }

        public static IEnumerable<DeckType> GetUserDecksFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.Where(x => x.Type == RoleTypes.DeckClaim).Select(x => (DeckType)Enum.Parse(typeof(DeckType), x.Value));
        }

        public static string GetUserAvatarFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.First(x => x.Type == RoleTypes.AvatarClaim).Value;
        }

        public static string GetUserLobbyFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.First(x => x.Type == RoleTypes.LobbyClaim).Value;
        }

        public static Guid GetPlayerIdFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            return Guid.Parse(claimsPrincipal.Claims.First(x => x.Type == RoleTypes.PlayerClaim).Value);
        }

        public static Guid GetGameIdFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            return Guid.Parse(claimsPrincipal.Claims.First(x => x.Type == RoleTypes.GameClaim).Value);
        }

        public static bool GetUserHasDeck(this ClaimsPrincipal claimsPrincipal, DeckType deck)
        {
            return claimsPrincipal.GetUserDecksFromJwt().Any(x => x == deck);
        }
    }
}
