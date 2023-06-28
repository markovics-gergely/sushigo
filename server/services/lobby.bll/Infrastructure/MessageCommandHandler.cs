using AutoMapper;
using lobby.bll.Infrastructure.Commands;
using lobby.bll.Infrastructure.Events;
using lobby.bll.Infrastructure.ViewModels;
using lobby.bll.Validators;
using lobby.dal.Domain;
using lobby.dal.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.bll.Extensions;
using shared.bll.Validators.Interfaces;

namespace lobby.bll.Infrastructure
{
    public class MessageCommandHandler :
        IRequestHandler<CreateMessageCommand, MessageViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private IValidator? _validator;

        public MessageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<MessageViewModel> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var lobby = _unitOfWork.LobbyRepository.Get(
                    includeProperties: nameof(Lobby.Players),
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.Message.LobbyId
                ).FirstOrDefault();
            if (lobby == null)
            {
                throw new EntityNotFoundException(nameof(AddPlayerCommand));
            }
            _validator = new OwnLobbyValidator(lobby, request.User);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(CreateMessageCommand));
            }
            var messageEntity = _mapper.Map<Message>(request.Message);
            messageEntity.DateTime = DateTime.Now;
            messageEntity.UserId = Guid.Parse(request.User?.GetUserIdFromJwt() ?? "");
            messageEntity.UserName = request.User?.GetUserNameFromJwt() ?? "";
            _unitOfWork.MessageRepository.Insert(messageEntity);
            await _unitOfWork.Save();
            var messageViewModel = _mapper.Map<MessageViewModel>(messageEntity);
            await _mediator.Publish(new AddMessageEvent(messageViewModel, request.Message.LobbyId), cancellationToken);
            return messageViewModel;
        }
    }
}
