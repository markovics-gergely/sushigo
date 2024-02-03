using lobby.dal.Domain;
using shared.bll.Extensions;
using shared.bll.Validators.Interfaces;
using System.Security.Claims;

namespace lobby.bll.Validators
{
    public class LobbyJoinValidator : IValidator
    {
        private readonly Lobby _lobby;
        private readonly ClaimsPrincipal? _user;

        public LobbyJoinValidator(Lobby lobby, ClaimsPrincipal? user)
        {
            _lobby = lobby;
            _user = user;
        }

        public bool Validate()
        {
            var guid = Guid.Parse(_user?.GetUserIdFromJwt() ?? "");
            return _lobby.Players.All(p => p.UserId != guid);
        }
    }
}
