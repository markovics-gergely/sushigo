using shared.Models;
using user.bll.Validators.Interfaces;

namespace user.bll.Validators.Implementations
{
    public class ClaimDeckValidator : IValidator
    {
        private readonly ICollection<DeckType> _deckTypes;
        private readonly DeckType _deckType;
        public ClaimDeckValidator(ICollection<DeckType> deckTypes, DeckType deckType) {
            _deckTypes = deckTypes;
            _deckType = deckType;
        }
        public bool Validate()
        {
            return !_deckTypes.Contains(_deckType);
        }
    }
}
