using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shop.bll.Infrastructure.Queries;
using shop.bll.Infrastructure.ViewModels;
using shop.dal.Domain;
using shop.dal.UnitOfWork.Interfaces;

namespace shop.bll.Infrastructure
{
    public class ShopQueryHandler :
        IRequestHandler<GetDecksQuery, IEnumerable<DeckViewModel>>,
        IRequestHandler<GetDeckQuery, DeckItemViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShopQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<DeckViewModel>> Handle(GetDecksQuery request, CancellationToken cancellationToken)
        {
            if (request.User == null)
            {
                throw new EntityNotFoundException(nameof(request.User));
            }
            var decks = _unitOfWork.DeckRepository.Get(
                    includeProperties: nameof(Deck.Cards),
                    transform: x => x.AsNoTracking()
                ).ToList();
            var deckViewModel = _mapper.Map<IEnumerable<DeckViewModel>>(decks);
            return Task.FromResult(deckViewModel);
        }

        public Task<DeckItemViewModel> Handle(GetDeckQuery request, CancellationToken cancellationToken)
        {
            var deck = _unitOfWork.DeckRepository.Get(
                filter: x => x.DeckType == request.DeckType,
                includeProperties: nameof(Deck.Cards),
                transform: x => x.AsNoTracking()
            ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(GetDeckQuery));
            var deckViewModel = _mapper.Map<DeckItemViewModel>(deck);
            return Task.FromResult(deckViewModel);
        }
    }
}
