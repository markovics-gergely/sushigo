using shared.Logic.Validators.Interfaces;

namespace shared.Logic.Validators.Implementations
{
    public class OrCondition : IValidator
    {
        private readonly IValidator validator1;
        private readonly IValidator validator2;

        public OrCondition(IValidator validator1, IValidator validator2)
        {
            this.validator1 = validator1;
            this.validator2 = validator2;
        }

        public bool Validate()
        {
            return validator1.Validate() || validator2.Validate();
        }
    }
}
