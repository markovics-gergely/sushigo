using Microsoft.EntityFrameworkCore;
using shared.bll.Validators.Interfaces;
using user.dal.UnitOfWork.Interfaces;

namespace user.bll.Validators
{
    public class RemoveFriendValidator : IValidator
    {
        private readonly Guid _userId;
        private readonly Guid _friendId;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveFriendValidator(Guid userId, Guid friendId, IUnitOfWork unitOfWork)
        {
            _userId = userId;
            _friendId = friendId;
            _unitOfWork = unitOfWork;
        }

        public bool Validate()
        {
            return _unitOfWork.FriendRepository.Get(
                    filter: x => x.OwnFriend(_userId, _friendId),
                    transform: x => x.AsNoTracking()
                ).Any();
        }
    }
}
