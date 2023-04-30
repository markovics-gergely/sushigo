using AutoMapper;
using lobby.bll.Exceptions;
using lobby.bll.Extensions;
using lobby.bll.Infrastructure.Commands;
using lobby.bll.Infrastructure.Events;
using lobby.bll.Infrastructure.Queries;
using lobby.bll.Infrastructure.ViewModels;
using lobby.bll.Validators.Implementations;
using lobby.bll.Validators.Interfaces;
using lobby.dal.Domain;
using lobby.dal.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace lobby.bll.Infrastructure
{
    public class LobbyCommandHandler :
        IRequestHandler<CreateLobbyCommand, LobbyViewModel>,
        IRequestHandler<AddPlayerCommand, LobbyViewModel>,
        IRequestHandler<RemovePlayerCommand, LobbyViewModel?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private IValidator? _validator;

        public LobbyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<LobbyViewModel> Handle(CreateLobbyCommand request, CancellationToken cancellationToken)
        {
            var guid = Guid.Parse(request.User?.GetUserIdFromJwt() ?? "");
            var lobbyEntity = _mapper.Map<Lobby>(request.Lobby);
            lobbyEntity.CreatorId = guid;
            lobbyEntity.Created = DateTime.UtcNow;
            _unitOfWork.LobbyRepository.Insert(lobbyEntity);
            await _unitOfWork.Save();
            var playerEntity = new Player
            {
                ImagePath = request.Lobby.CreatorImagePath,
                UserId = guid,
                UserName = request.User?.GetUserNameFromJwt() ?? "",
                LobbyId = lobbyEntity.Id
            };
            _unitOfWork.PlayerRepository.Insert(playerEntity);
            lobbyEntity.Players.Add(playerEntity);
            await _unitOfWork.Save();
            await _mediator.Publish(new AddLobbyEvent(_mapper.Map<LobbyItemViewModel>(lobbyEntity)), cancellationToken);
            return _mapper.Map<LobbyViewModel>(lobbyEntity);
        }

        public async Task<LobbyViewModel> Handle(AddPlayerCommand request, CancellationToken cancellationToken)
        {
            var lobby = _unitOfWork.LobbyRepository.Get(
                    includeProperties: nameof(Lobby.Players),
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.PlayerDTO.LobbyId
                ).FirstOrDefault();
            if (lobby == null)
            {
                throw new EntityNotFoundException(nameof(AddPlayerCommand));
            }
            var playerEntity = _mapper.Map<Player>(request.PlayerDTO);
            _unitOfWork.PlayerRepository.Insert(playerEntity);
            lobby.Players.Add(playerEntity);
            await _unitOfWork.Save();
            var playerViewModel = _mapper.Map<PlayerViewModel>(playerEntity);
            await _mediator.Publish(new AddPlayerEvent(request.PlayerDTO.LobbyId, playerViewModel), cancellationToken);
            return _mapper.Map<LobbyViewModel>(lobby);
        }

        public async Task<LobbyViewModel?> Handle(RemovePlayerCommand request, CancellationToken cancellationToken)
        {
            var lobby = _unitOfWork.LobbyRepository.Get(
                    includeProperties: nameof(Lobby.Players),
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.RemovePlayerDTO.LobbyId
                ).FirstOrDefault();
            if (lobby == null)
            {
                throw new EntityNotFoundException(nameof(Lobby));
            }
            _validator = new AndCondition(
                new OwnLobbyValidator(lobby, request.User),
                new OrCondition(
                    new LobbyCreatorValidator(lobby, request.User),
                    new OwnPlayerValidator(request.RemovePlayerDTO.PlayerId, request.User)
                )
            );
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(RemovePlayerCommand));
            }
            if (request.RemovePlayerDTO.PlayerId == lobby.CreatorId)
            {
                var messages = _unitOfWork.MessageRepository.Get(
                        filter: x => x.LobbyId == request.RemovePlayerDTO.LobbyId,
                        transform: x => x.AsNoTracking()
                    ).Select(m => m.Id).ToList();
                foreach (var message in messages)
                {
                    _unitOfWork.MessageRepository.Delete(message);
                }
                foreach (var player in lobby.Players)
                {
                    _unitOfWork.PlayerRepository.Delete(player.Id);
                }
                _unitOfWork.LobbyRepository.Delete(lobby.Id);
                await _unitOfWork.Save();
                await _mediator.Publish(new RemoveLobbyEvent(request.RemovePlayerDTO.LobbyId), cancellationToken);
                return null;
            }
            else
            {
                _unitOfWork.PlayerRepository.Delete(request.RemovePlayerDTO.PlayerId);
                lobby.Players = lobby.Players.Where(p => p.Id != request.RemovePlayerDTO.PlayerId).ToList();
                await _unitOfWork.Save();
                await _mediator.Publish(
                    new RemovePlayerEvent(request.RemovePlayerDTO.LobbyId, request.RemovePlayerDTO.PlayerId),
                    cancellationToken
                );
                return _mapper.Map<LobbyViewModel>(lobby);
            }
        }
    }
}
