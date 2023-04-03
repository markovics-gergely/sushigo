using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using shop.bll.Exceptions;
using shop.bll.Extensions;
using shop.bll.Infrastructure.Queries;
using shop.bll.Infrastructure.ViewModels;
using shop.bll.Validators.Interfaces;
using shop.dal.Domain;
using shop.dal.UnitOfWork.Interfaces;

namespace shop.bll.Infrastructure
{
    public class ShopQueryHandler :
        IRequestHandler<GetDecksQuery, IEnumerable<DeckViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IValidator? _validator;

        public ShopQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<DeckViewModel>> Handle(GetDecksQuery request, CancellationToken cancellationToken)
        {
            if (request.User == null)
            {
                throw new EntityNotFoundException("Requested user not found");
            }
            var decks = _unitOfWork.DeckRepository.Get(
                    includeProperties: nameof(Deck.Cards),
                    transform: x => x.AsNoTracking()
                ).ToList();
            var deckViewModel = _mapper.Map<IEnumerable<DeckViewModel>>(decks);
            var claims = request.User.GetUserDecksFromJwt();
            foreach(var deck in deckViewModel)
            {
                deck.Claimed = claims.Contains(deck.DeckType);
            }
            return Task.FromResult(deckViewModel);
        }
    }
}
