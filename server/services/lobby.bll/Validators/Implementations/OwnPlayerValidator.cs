using lobby.bll.Extensions;
using lobby.bll.Validators.Interfaces;
using System.Security.Claims;

namespace lobby.bll.Validators.Implementations
{
    public class OwnPlayerValidator : IValidator
    {
        private readonly Guid _playerUserId;
        private readonly ClaimsPrincipal? _user;

        public OwnPlayerValidator(Guid playerId, ClaimsPrincipal? user)
        {
            _playerUserId = playerId;
            _user = user;
        }

        public bool Validate()
        {
            return _playerUserId.ToString() == _user?.GetUserIdFromJwt();
        }
    }
}
