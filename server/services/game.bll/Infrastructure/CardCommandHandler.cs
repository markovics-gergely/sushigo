using AutoMapper;
using game.bll.Infrastructure.Commands;
using game.bll.Infrastructure.Commands.Card.Abstract;
using game.dal.UnitOfWork.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.bll.Extensions;
using shared.dal.Models;

namespace game.bll.Infrastructure
{
    public class CardCommandHandler :
        IRequestHandler<PlayCardCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CardCommandHandler(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _mediator = mediator;
        }

        public Task Handle(PlayCardCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null) throw new EntityNotFoundException(nameof(request.User));
            var player = _unitOfWork.PlayerRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == request.User.GetPlayerIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(SimpleAddPoint));
            var cardCommand = GetCommand(request.CardType.GetClass());
            if (cardCommand == null) return Task.CompletedTask;
            return Task.CompletedTask;
        }
        private ICardCommand<TCard>? GetCommand<TCard>(TCard card) where TCard : CardTypeWrapper
        {
            return (ICardCommand<TCard>?) _serviceProvider.GetService(typeof(ICardCommand<>).MakeGenericType(card.GetType()));
        }
    }
}
