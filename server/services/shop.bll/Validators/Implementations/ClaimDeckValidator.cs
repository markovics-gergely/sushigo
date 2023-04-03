using shared.Models;
using shop.bll.Validators.Interfaces;

namespace shop.bll.Validators.Implementations
{
    public class ClaimDeckValidator : IValidator
    {
        private readonly ICollection<DeckType> _deckTypes;
        private readonly DeckType _deckType;
        public ClaimDeckValidator(ICollection<DeckType> deckTypes, DeckType deckType)
        {
            _deckTypes = deckTypes;
            _deckType = deckType;
        }
        public bool Validate()
        {
            return !_deckTypes.Contains(_deckType);
        }
    }
}
