using shared.bll.Validators.Interfaces;

namespace shop.bll.Validators
{
    public class ClaimValidator : IValidator
    {
        private readonly long _expMin;
        private readonly long _exp;
        public ClaimValidator(long expMin, long exp)
        {
            _expMin = expMin;
            _exp = exp;
        }
        public bool Validate()
        {
            return _exp >= _expMin;
        }
    }
}
