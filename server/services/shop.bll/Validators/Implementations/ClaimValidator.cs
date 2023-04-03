using shop.bll.Validators.Interfaces;

namespace shop.bll.Validators.Implementations
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
