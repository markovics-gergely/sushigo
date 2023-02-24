using user.bll.Validators.Interfaces;
using user.dal.Types;

namespace user.bll.Validators.Implementations
{
    public class ClaimGameValidator : IValidator
    {
        private readonly ICollection<GameTypes> _gameTypes;
        private readonly GameTypes _gameType;
        public ClaimGameValidator(ICollection<GameTypes> gameTypes, GameTypes gameType) {
            _gameTypes = gameTypes;
            _gameType = gameType;
        }
        public bool Validate()
        {
            return !_gameTypes.Contains(_gameType);
        }
    }
}
