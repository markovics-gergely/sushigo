using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using user.bll.Extensions;
using user.bll.Infrastructure.Queries;
using user.bll.Infrastructure.ViewModels;
using user.bll.Validators.Interfaces;
using user.dal.Domain;
using user.dal.Types;
using user.dal.UnitOfWork.Interfaces;

namespace user.bll.Infrastructure
{
    public class FriendQueryHandler :
        IRequestHandler<GetFriendsQuery, FriendListViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IValidator? _validator;

        public FriendQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private IEnumerable<ApplicationUser> GetUsersWithIds(IEnumerable<Guid>? guids)
        {
            return guids != null ? _unitOfWork.UserRepository.Get(
                    filter: x => guids.Contains(x.Id),
                    transform: x => x.AsNoTracking()
                ) : new List<ApplicationUser>();
        }

        public Task<FriendListViewModel> Handle(GetFriendsQuery request, CancellationToken cancellationToken)
        {
            var userguid = Guid.Parse(request.User?.GetUserIdFromJwt() ?? "");
            var friends = _unitOfWork.FriendRepository.Get(
                filter: x => userguid == x.SenderId || userguid == x.ReceiverId,
                transform: x => x.AsNoTracking()
            )
                .GroupBy(f => f.GetFriendTypes(userguid))
                .ToDictionary(f => f.Key, f => f.Select(ff => ff.SenderId == userguid ? ff.ReceiverId : ff.SenderId).ToList());
            return Task.FromResult(new FriendListViewModel
            {
                Friends = _mapper.Map<IEnumerable<UserNameViewModel>>(GetUsersWithIds(friends.TryGetValue(FriendTypes.Friend, out var val1) ? val1 : new())),
                Sent = _mapper.Map<IEnumerable<UserNameViewModel>>(GetUsersWithIds(friends.TryGetValue(FriendTypes.Sent, out var val2) ? val2 : new())),
                Received = _mapper.Map<IEnumerable<UserNameViewModel>>(GetUsersWithIds(friends.TryGetValue(FriendTypes.Received, out var val3) ? val3 : new())),
            });
        }
    }
}
