using game.dal.Domain;
using shared.bll.Extensions;
using shared.bll.Validators.Interfaces;
using System.Security.Claims;

namespace game.bll.Validators
{
    public class ActualPlayerValidator : IValidator
    {
        private readonly Game _game;
        private readonly ClaimsPrincipal? _user;

        public ActualPlayerValidator(Game game, ClaimsPrincipal? user)
        {
            _game = game;
            _user = user;
        }

        public bool Validate()
        {
            return _game.ActualPlayerId == _user?.GetPlayerIdFromJwt();
        }
    }
}
