using lobby.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Validators.Interfaces;

namespace lobby.bll.Validators
{
    public class LobbyNameValidator : IValidator
    {
        private readonly string _name;
        private readonly IUnitOfWork _unitOfWork;

        public LobbyNameValidator(string name, IUnitOfWork unitOfWork)
        {
            _name = name;
            _unitOfWork = unitOfWork;
        }

        public bool Validate()
        {
            return !_unitOfWork.LobbyRepository.Get(
                    filter: x => x.Name == _name,
                    transform: x => x.AsNoTracking()
                ).Any();
        }
    }
}
