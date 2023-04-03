﻿using shared.Models;
using System.Security.Claims;

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

        public static IEnumerable<DeckType> GetUserDecksFromJwt(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.Where(x => x.Type == RoleTypes.GameClaim).Select(x => (DeckType)Enum.Parse(typeof(DeckType), x.Value));
        }

        public static bool GetUserHasDeck(this ClaimsPrincipal claimsPrincipal, DeckType deck)
        {
            return claimsPrincipal.GetUserDecksFromJwt().Any(x => x == deck);
        }
    }
}