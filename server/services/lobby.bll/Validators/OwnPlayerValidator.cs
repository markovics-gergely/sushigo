using shared.bll.Extensions;
using shared.bll.Validators.Interfaces;
using System.Security.Claims;

namespace lobby.bll.Validators
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
