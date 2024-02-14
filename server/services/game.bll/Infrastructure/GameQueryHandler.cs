using AutoMapper;
using game.bll.Infrastructure.Queries;
using game.bll.Infrastructure.ViewModels;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.bll.Extensions;

namespace game.bll.Infrastructure
{
    public class GameQueryHandler : 
        IRequestHandler<GetGameQuery, GameViewModel>,
        IRequestHandler<GetHandQuery, HandViewModel>,
        IRequestHandler<GetOwnHandQuery, HandViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GameQueryHandler(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _mediator = mediator;
        }

        public Task<GameViewModel> Handle(GetGameQuery request, CancellationToken cancellationToken)
        {
            // Get game entity
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User!.GetGameIdFromJwt(),
                    includeProperties: "Players.Board.Cards,Players.SelectedCardInfo"
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Game));

            return Task.FromResult(_mapper.Map<GameViewModel>(game));
        }

        public Task<HandViewModel> Handle(GetHandQuery request, CancellationToken cancellationToken)
        {
            // Get card entities
            var cards = _unitOfWork.HandCardRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.HandId == request.HandId,
                    includeProperties: nameof(HandCard.CardInfo)
                ).ToList();

            return Task.FromResult(new HandViewModel
            {
                Cards = _mapper.Map<IEnumerable<HandCardViewModel>>(cards)
            });
        }

        public Task<HandViewModel> Handle(GetOwnHandQuery request, CancellationToken cancellationToken)
        {
            if (request.User == null) throw new EntityNotFoundException(nameof(request.User));

            // Get player entity
            var player = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User.GetPlayerIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Player));

            // Get card entities
            var cards = _unitOfWork.HandCardRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => player.HandId == x.HandId,
                    includeProperties: nameof(HandCard.CardInfo)
                ).ToList();

            return Task.FromResult(new HandViewModel
            {
                Cards = _mapper.Map<IEnumerable<HandCardViewModel>>(cards)
            });
        }
    }
}
