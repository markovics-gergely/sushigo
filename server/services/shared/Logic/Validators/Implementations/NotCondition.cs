using shared.Logic.Validators.Interfaces;

namespace shared.Logic.Validators.Implementations
{
    public class NotCondition : IValidator
    {
        private readonly IValidator validator;

        public NotCondition(IValidator validator)
        {
            this.validator = validator;
        }

        public bool Validate()
        {
            return !validator.Validate();
        }
    }
}
