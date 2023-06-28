using AutoMapper;
using lobby.bll.Infrastructure.Queries;
using lobby.bll.Infrastructure.ViewModels;
using lobby.dal.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shared.bll.Validators.Interfaces;

namespace lobby.bll.Infrastructure
{
    public class MessageQueryHandler :
        IRequestHandler<GetMessagesQuery, IEnumerable<MessageViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<IEnumerable<MessageViewModel>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = _unitOfWork.MessageRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.LobbyId == request.LobbyId
                    ).ToList();
            return Task.FromResult(_mapper.Map<IEnumerable<MessageViewModel>>(messages));
        }
    }
}
