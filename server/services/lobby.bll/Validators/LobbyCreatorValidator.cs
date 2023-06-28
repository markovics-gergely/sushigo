using lobby.dal.Domain;
using shared.bll.Extensions;
using shared.bll.Validators.Interfaces;
using System.Security.Claims;

namespace lobby.bll.Validators
{
    public class LobbyCreatorValidator : IValidator
    {
        private readonly Lobby _lobby;
        private readonly ClaimsPrincipal? _user;

        public LobbyCreatorValidator(Lobby lobby, ClaimsPrincipal? user)
        {
            _lobby = lobby;
            _user = user;
        }

        public bool Validate()
        {
            return _lobby.CreatorUserId.ToString() == _user?.GetUserIdFromJwt();
        }
    }
}
