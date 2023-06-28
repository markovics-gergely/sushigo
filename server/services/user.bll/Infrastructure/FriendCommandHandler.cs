using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.bll.Extensions;
using user.bll.Infrastructure.Commands;
using user.bll.Infrastructure.Events;
using user.bll.Infrastructure.ViewModels;
using user.dal.Domain;
using user.dal.UnitOfWork.Interfaces;

namespace user.bll.Infrastructure
{
    public class FriendCommandHandler :
        IRequestHandler<AddFriendCommand, UserNameViewModel>,
        IRequestHandler<RemoveFriendCommand>,
        IRequestHandler<UpdateFriendOfflineCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public FriendCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<UserNameViewModel> Handle(AddFriendCommand request, CancellationToken cancellationToken)
        {
            var friendUserEntity = _unitOfWork.UserRepository.Get(
                filter: x => x.UserName == request.FriendName,
                transform: x => x.AsNoTracking()).FirstOrDefault();
            if (friendUserEntity == null)
            {
                throw new EntityNotFoundException(nameof(friendUserEntity) + " user not found");
            }
            var userguid = Guid.Parse(request.User?.GetUserIdFromJwt() ?? "");
            var friendEntity = _unitOfWork.FriendRepository.Get(filter: x =>
                (x.SenderId == userguid && x.ReceiverId == friendUserEntity.Id)
                || (x.SenderId == friendUserEntity.Id && x.ReceiverId == userguid)
            ).FirstOrDefault();
            if (friendEntity != null)
            {
                if (friendEntity.Pending && friendEntity.ReceiverId == userguid)
                {
                    friendEntity.Pending = false;
                    _unitOfWork.FriendRepository.Update(friendEntity);
                } else
                {
                    throw new ValidationErrorException("Invalid friend request");
                }
            }
            else
            {
                var newEntity = new Friend
                {
                    Pending = true,
                    SenderId = userguid,
                    ReceiverId = friendUserEntity.Id,
                };
                _unitOfWork.FriendRepository.Insert(newEntity);
            }
            await _unitOfWork.Save();
            var model = _mapper.Map<UserNameViewModel>(friendUserEntity);
            await _mediator.Publish(new AddFriendEvent
            { ReceiverId = friendUserEntity.Id, SenderUser = new UserNameViewModel
                { 
                    Id = userguid,
                    UserName = request.User?.GetUserNameFromJwt() ?? ""
                }
            }, cancellationToken);
            return model;
        }

        public async Task Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
        {
            var userguid = Guid.Parse(request.User?.GetUserIdFromJwt() ?? "");
            var friend = _unitOfWork.FriendRepository.Get(
                    filter: x =>
                        (x.SenderId == userguid && x.ReceiverId == request.FriendId)
                        || (x.SenderId == request.FriendId && x.ReceiverId == userguid),
                    transform: x => x.AsNoTracking()
                ).FirstOrDefault();
            if (friend == null)
            {
                throw new EntityNotFoundException("Friend not found");
            }
            _unitOfWork.FriendRepository.Delete(friend);
            await _unitOfWork.Save();
            await _mediator.Publish(new RemoveFriendEvent
            {
                ReceiverId = request.FriendId,
                SenderId = userguid
            }, cancellationToken);
        }

        public async Task Handle(UpdateFriendOfflineCommand request, CancellationToken cancellationToken)
        {
            var friends = _unitOfWork.FriendRepository.Get(
                filter: x => request.UserId == x.SenderId.ToString() || request.UserId == x.ReceiverId.ToString(),
                transform: x => x.AsNoTracking()
            ).Select(x => x.SenderId.ToString() == request.UserId ? x.ReceiverId.ToString() : x.SenderId.ToString()).ToList();
            if (friends.Any())
            {
                await _mediator.Publish(new OfflineEvent { Friends = friends, UserId = request.UserId }, cancellationToken);
            }
        }
    }
}
