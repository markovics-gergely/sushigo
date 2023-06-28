using lobby.dal.Domain;
using shared.bll.Extensions;
using shared.bll.Validators.Interfaces;
using System.Security.Claims;

namespace lobby.bll.Validators
{
    public class OwnLobbyValidator : IValidator
    {
        private readonly Lobby _lobby;
        private readonly ClaimsPrincipal? _user;

        public OwnLobbyValidator(Lobby lobby, ClaimsPrincipal? user)
        {
            _lobby = lobby;
            _user = user;
        }

        public bool Validate()
        {
            return _lobby.Players.Any(p => p.UserId.ToString() == _user?.GetUserIdFromJwt());
        }
    }
}
