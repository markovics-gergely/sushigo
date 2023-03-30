namespace shop.bll.Extensions
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

        public static IEnumerable<DeckType> GetUserGamesFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.Where(x => x.Type == RoleTypes.GameClaim).Select(x => (GameTypes)Enum.Parse(typeof(GameTypes), x.Value));
        }

        public static bool GetUserHasGame(this ClaimsPrincipal claimsPrincipal, GameTypes game)
        {
            return claimsPrincipal.GetUserGamesFromJwt().Any(x => x == game);
        }
    }
}
