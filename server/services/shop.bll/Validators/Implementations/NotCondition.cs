using shop.bll.Validators.Interfaces;

namespace shop.bll.Validators.Implementations
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
