using game.dal.Domain;
using game.dal.Types;
using shared.bll.Validators.Interfaces;

namespace game.bll.Validators
{
    public class PhaseValidator : IValidator
    {
        private readonly Game _game;
        private readonly Phase _phase;

        public PhaseValidator(Game game, Phase phase)
        {
            _game = game;
            _phase = phase;
        }

        public bool Validate()
        {
            return _game.Phase == _phase;
        }
    }
}
