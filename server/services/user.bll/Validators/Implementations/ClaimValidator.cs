using user.bll.Validators.Interfaces;

namespace user.bll.Validators.Implementations
{
    public class ClaimValidator : IValidator
    {
        private readonly int _expMin;
        private readonly int _exp;
        public ClaimValidator(int expMin, int exp) {
            _expMin = expMin;
            _exp = exp;
        }
        public bool Validate()
        {
            return _exp >= _expMin;
        }
    }
}
