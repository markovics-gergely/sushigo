using lobby.bll.Extensions;
using lobby.bll.Validators.Interfaces;
using System.Security.Claims;

namespace lobby.bll.Validators.Implementations
{
    public class OwnPlayerValidator : IValidator
    {
        private readonly Guid _playerId;
        private readonly ClaimsPrincipal? _user;

        public OwnPlayerValidator(Guid playerId, ClaimsPrincipal? user)
        {
            _playerId = playerId;
            _user = user;
        }

        public bool Validate()
        {
            return _playerId.ToString() == _user?.GetUserIdFromJwt();
        }
    }
}
