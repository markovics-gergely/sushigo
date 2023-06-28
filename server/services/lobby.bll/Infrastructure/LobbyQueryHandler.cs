using AutoMapper;
using lobby.bll.Infrastructure.Queries;
using lobby.bll.Infrastructure.ViewModels;
using lobby.bll.Validators;
using lobby.dal.Domain;
using lobby.dal.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.bll.Validators.Interfaces;

namespace lobby.bll.Infrastructure
{
    public class LobbyQueryHandler :
        IRequestHandler<GetLobbiesQuery, IEnumerable<LobbyItemViewModel>>,
        IRequestHandler<GetLobbyQuery, LobbyViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IValidator? _validator;

        public LobbyQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<IEnumerable<LobbyItemViewModel>> Handle(GetLobbiesQuery request, CancellationToken cancellationToken)
        {
            var lobbies = _unitOfWork.LobbyRepository.Get(
                    transform: x => x.AsNoTracking()
                ).ToList();
            var lobbiesViewModel = _mapper.Map<IEnumerable<LobbyItemViewModel>>(lobbies);
            return Task.FromResult(lobbiesViewModel);
        }

        public Task<LobbyViewModel> Handle(GetLobbyQuery request, CancellationToken cancellationToken)
        {
            var lobby = _unitOfWork.LobbyRepository.Get(
                    includeProperties: nameof(Lobby.Players),
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.LobbyId
                ).FirstOrDefault();
            if (lobby == null )
            {
                throw new EntityNotFoundException(nameof(Lobby));
            }
            _validator = new OwnLobbyValidator(lobby, request.User);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("No permission for lobby");
            }
            return Task.FromResult(_mapper.Map<LobbyViewModel>(lobby));
        }
    }
}
