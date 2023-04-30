using lobby.bll.Extensions;
using lobby.bll.Validators.Interfaces;
using lobby.dal.Domain;
using System.Security.Claims;

namespace lobby.bll.Validators.Implementations
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
            return _lobby.Players.Any(p => p.Id.ToString() == _user?.GetUserIdFromJwt());
        }
    }
}
