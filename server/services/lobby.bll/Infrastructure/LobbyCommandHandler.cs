using AutoMapper;
using lobby.bll.Infrastructure.Commands;
using lobby.bll.Infrastructure.Events;
using lobby.bll.Infrastructure.ViewModels;
using lobby.bll.Validators;
using lobby.dal.Domain;
using lobby.dal.UnitOfWork.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.bll.Extensions;
using shared.bll.Validators.Implementations;
using shared.bll.Validators.Interfaces;
using shared.dal.Models;

namespace lobby.bll.Infrastructure
{
    public class LobbyCommandHandler :
        IRequestHandler<CreateLobbyCommand, LobbyViewModel>,
        IRequestHandler<AddPlayerCommand, LobbyViewModel>,
        IRequestHandler<RemovePlayerCommand, LobbyViewModel?>,
        IRequestHandler<JoinLobbyCommand, LobbyViewModel>,
        IRequestHandler<UpdateLobbyDeckCommand, LobbyViewModel>,
        IRequestHandler<PlayerReadyCommand, LobbyViewModel>,
        IRequestHandler<RemoveLobbyCommand, LobbyViewModel?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _endpoint;
        private IValidator? _validator;

        public LobbyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator, IPublishEndpoint endpoint)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
            _endpoint = endpoint;
        }

        public async Task<LobbyViewModel> Handle(CreateLobbyCommand request, CancellationToken cancellationToken)
        {
            // Prevent creating a lobby with the same name
            _validator = new LobbyNameValidator(request.Lobby.Name, _unitOfWork);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(CreateLobbyCommand));
            }

            // Build the lobby entity
            var guid = Guid.Parse(request.User?.GetUserIdFromJwt() ?? "");
            var lobbyEntity = _mapper.Map<Lobby>(request.Lobby);
            lobbyEntity.CreatorUserId = guid;
            lobbyEntity.Created = DateTime.UtcNow;
            lobbyEntity.DeckType = DeckType.SushiGo;

            // Save the lobby
            _unitOfWork.LobbyRepository.Insert(lobbyEntity);
            await _unitOfWork.Save();

            // Build the creator player entity
            var playerEntity = new Player
            {
                ImagePath = request.User?.GetUserAvatarFromJwt(),
                UserId = guid,
                UserName = request.User?.GetUserNameFromJwt() ?? "",
                LobbyId = lobbyEntity.Id
            };

            // Save the creator player
            _unitOfWork.PlayerRepository.Insert(playerEntity);
            lobbyEntity.Players.Add(playerEntity);
            await _unitOfWork.Save();

            // Send the lobby to SignalR and cache
            var lobbyViewModel = _mapper.Map<LobbyViewModel>(lobbyEntity);
            await _mediator.Publish(new AddLobbyEvent(lobbyViewModel), cancellationToken);

            // Send the player data to RabbitMQ
            await _endpoint.Publish(new LobbyJoinedDTO
            {
                UserId = guid,
                LobbyId = lobbyEntity.Id
            }, cancellationToken);

            return lobbyViewModel;
        }

        public async Task<LobbyViewModel> Handle(AddPlayerCommand request, CancellationToken cancellationToken)
        {
            // Get lobby and player entities
            var lobby = _unitOfWork.LobbyRepository.Get(
                    includeProperties: nameof(Lobby.Players),
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.PlayerDTO.LobbyId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Lobby));

            // Build new player entity
            var playerEntity = _mapper.Map<Player>(request.PlayerDTO);
            _unitOfWork.PlayerRepository.Insert(playerEntity);
            lobby.Players.Add(playerEntity);
            await _unitOfWork.Save();

            // Send the player to SignalR
            var playerViewModel = _mapper.Map<PlayerViewModel>(playerEntity);
            await _mediator.Publish(new AddPlayerEvent(request.PlayerDTO.LobbyId, playerViewModel), cancellationToken);

            // Send player data to RabbitMQ
            await _endpoint.Publish(new LobbyJoinedDTO
            {
                UserId = request.PlayerDTO.UserId,
                LobbyId = request.PlayerDTO.LobbyId
            }, cancellationToken);

            var lobbyViewModel = _mapper.Map<LobbyViewModel>(lobby);
            // Send the lobby to the cache
            await _mediator.Publish(new RefreshLobbyEvent(lobbyViewModel), cancellationToken);
            return lobbyViewModel;
        }

        public async Task<LobbyViewModel?> Handle(RemovePlayerCommand request, CancellationToken cancellationToken)
        {
            // Get lobby and player entities
            var lobby = _unitOfWork.LobbyRepository.Get(
                    includeProperties: nameof(Lobby.Players),
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.RemovePlayerDTO.LobbyId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Lobby));

            // Find the player to remove
            var player = lobby.Players.FirstOrDefault(p => p.Id == request.RemovePlayerDTO.PlayerId)
                ?? throw new EntityNotFoundException(nameof(Player));

            // Validate the right to remove the player
            _validator = new AndCondition(
                new OwnLobbyValidator(lobby, request.User),
                new OrCondition(
                    new LobbyCreatorValidator(lobby, request.User),
                    new OwnPlayerValidator(player.UserId, request.User)
                )
            );
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(RemovePlayerCommand));
            }

            // If the player is the creator, remove the whole lobby
            if (player.UserId == lobby.CreatorUserId)
            {
                var messages = _unitOfWork.MessageRepository.Get(
                        filter: x => x.LobbyId == request.RemovePlayerDTO.LobbyId,
                        transform: x => x.AsNoTracking()
                    ).Select(m => m.Id).ToList();
                foreach (var message in messages)
                {
                    _unitOfWork.MessageRepository.Delete(message);
                }
                foreach (var p in lobby.Players)
                {
                    _unitOfWork.PlayerRepository.Delete(p.Id);
                    // Send removed player data to RabbitMQ
                    await _endpoint.Publish(new LobbyJoinedDTO
                    {
                        UserId = p.UserId,
                        LobbyId = null
                    }, cancellationToken);
                }
                _unitOfWork.LobbyRepository.Delete(lobby.Id);
                await _unitOfWork.Save();
                // Send the removed lobby to SignalR and cache
                await _mediator.Publish(new RemoveLobbyEvent(request.RemovePlayerDTO.LobbyId), cancellationToken);
                return null;
            }
            // If the player is not the creator, remove only the player
            else
            {
                _unitOfWork.PlayerRepository.Delete(request.RemovePlayerDTO.PlayerId);
                lobby.Players = lobby.Players.Where(p => p.Id != request.RemovePlayerDTO.PlayerId).ToList();
                await _unitOfWork.Save();

                // Send the removed player to SignalR
                await _mediator.Publish(
                    new RemovePlayerEvent(request.RemovePlayerDTO.LobbyId, request.RemovePlayerDTO.PlayerId),
                    cancellationToken
                );

                // Send removed player data to RabbitMQ
                await _endpoint.Publish(new LobbyJoinedDTO
                {
                    UserId = player.UserId,
                    LobbyId = null
                }, cancellationToken);

                var lobbyViewModel = _mapper.Map<LobbyViewModel>(lobby);
                // Send the lobby to the cache
                await _mediator.Publish(new RefreshLobbyEvent(lobbyViewModel), cancellationToken);
                return lobbyViewModel;
            }
        }

        public async Task<LobbyViewModel> Handle(JoinLobbyCommand request, CancellationToken cancellationToken)
        {
            // Get lobby and player entities
            var lobby = _unitOfWork.LobbyRepository.Get(
                    includeProperties: nameof(Lobby.Players),
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.JoinLobbyDTO.Id
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Lobby));

            // Validate the right to join the lobby
            _validator = new LobbyJoinValidator(lobby, request.User);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(Lobby));
            }

            // Build new player entity
            var guid = Guid.Parse(request.User?.GetUserIdFromJwt() ?? "");
            var playerEntity = new Player
            {
                ImagePath = request.User?.GetUserAvatarFromJwt(),
                UserId = guid,
                UserName = request.User?.GetUserNameFromJwt() ?? "",
                LobbyId = request.JoinLobbyDTO.Id
            };
            _unitOfWork.PlayerRepository.Insert(playerEntity);
            await _unitOfWork.Save();

            // Add the player to the lobby
            lobby.Players.Add(playerEntity);
            await _unitOfWork.Save();

            // Send the player to SignalR
            var playerViewModel = _mapper.Map<PlayerViewModel>(playerEntity);
            await _mediator.Publish(new AddPlayerEvent(request.JoinLobbyDTO.Id, playerViewModel), cancellationToken);

            // Send player data to RabbitMQ
            await _endpoint.Publish(new LobbyJoinedDTO
            {
                UserId = guid,
                LobbyId = request.JoinLobbyDTO.Id
            }, cancellationToken);

            // Send the lobby to the cache
            var lobbyViewModel = _mapper.Map<LobbyViewModel>(lobby);
            await _mediator.Publish(new RefreshLobbyEvent(lobbyViewModel), cancellationToken);
            return lobbyViewModel;
        }

        public async Task<LobbyViewModel> Handle(UpdateLobbyDeckCommand request, CancellationToken cancellationToken)
        {
            // Get lobby and player entities
            var lobby = _unitOfWork.LobbyRepository.Get(
                    includeProperties: nameof(Lobby.Players),
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.UpdateLobbyDTO.LobbyId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Lobby));

            // Validate the right to update the lobby
            _validator = new AndCondition(
                new OwnLobbyValidator(lobby, request.User),
                new LobbyCreatorValidator(lobby, request.User)
            );
            if (!_validator.Validate()) throw new ValidationErrorException(nameof(UpdateLobbyDeckCommand));

            // Update the deck type of the lobby
            lobby.DeckType = request.UpdateLobbyDTO.DeckType;
            _unitOfWork.LobbyRepository.Update(lobby);
            foreach(var player in lobby.Players)
            {
                player.Ready = false;
                _unitOfWork.PlayerRepository.Update(player);
            }
            await _unitOfWork.Save();

            // Send the updated lobby to SignalR and cache
            var lobbyViewModel = _mapper.Map<LobbyViewModel>(lobby);
            await _mediator.Publish(new UpdateDeckTypeEvent(lobbyViewModel), cancellationToken);
            return lobbyViewModel;
        }

        public async Task<LobbyViewModel> Handle(PlayerReadyCommand request, CancellationToken cancellationToken)
        {
            // Get player entity
            var guid = Guid.Parse(request.User?.GetUserIdFromJwt() ?? "");
            var player = _unitOfWork.PlayerRepository.Get(
                    filter: x => x.Id == request.PlayerReadyDTO.PlayerId && x.UserId == guid
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(PlayerReadyCommand));

            // Set player ready status
            player.Ready = request.PlayerReadyDTO.Ready;
            _unitOfWork.PlayerRepository.Update(player);
            await _unitOfWork.Save();

            // Get lobby entity of the player
            var lobby = _unitOfWork.LobbyRepository.Get(
                    includeProperties: nameof(Lobby.Players),
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == player.LobbyId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Lobby));

            // Send the lobby to SignalR and cache
            var lobbyViewModel = _mapper.Map<LobbyViewModel>(lobby);
            await _mediator.Publish(new PlayerReadyEvent(lobbyViewModel), cancellationToken);
            return lobbyViewModel;
        }

        public async Task<LobbyViewModel?> Handle(RemoveLobbyCommand request, CancellationToken cancellationToken)
        {
            // Get lobby entity
            var lobby = _unitOfWork.LobbyRepository.Get(
                    includeProperties: nameof(Lobby.Players),
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.LobbyId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Lobby));

            // Get messages sent in the lobby
            var messages = _unitOfWork.MessageRepository.Get(
                        filter: x => x.LobbyId == request.LobbyId,
                        transform: x => x.AsNoTracking()
                    ).Select(m => m.Id).ToList();

            // Remove every message from the lobby
            foreach (var message in messages)
            {
                _unitOfWork.MessageRepository.Delete(message);
            }

            // Remove every player from the lobby
            foreach (var p in lobby.Players)
            {
                _unitOfWork.PlayerRepository.Delete(p.Id);
                await _endpoint.Publish(new LobbyJoinedDTO
                {
                    UserId = p.UserId,
                    LobbyId = null
                }, cancellationToken);
            }
            // Remove the lobby
            _unitOfWork.LobbyRepository.Delete(lobby.Id);
            await _unitOfWork.Save();
            await _mediator.Publish(new RemoveLobbyEvent(request.LobbyId), cancellationToken);
            return null;
        }
    }
}
