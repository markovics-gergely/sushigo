using AutoMapper;
using MassTransit;
using MediatR;
using shared.Models;
using shop.bll.Exceptions;
using shop.bll.Extensions;
using shop.bll.Infrastructure.Commands;
using shop.bll.Validators.Implementations;
using shop.bll.Validators.Interfaces;
using shop.dal.UnitOfWork.Interfaces;

namespace shop.bll.Infrastructure
{
    public class ShopCommandHandler :
        IRequestHandler<BuyDeckCommand>,
        IRequestHandler<BuyPartyCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _endpoint;
        private IValidator? _validator;

        public ShopCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator, IPublishEndpoint endpoint)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
            _endpoint = endpoint;
        }

        public async Task Handle(BuyDeckCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            _validator = new AndCondition(
                    new ClaimValidator(RoleTypes.GameExp, request.User?.GetUserExpFromJwt() ?? 0),
                    new ClaimDeckValidator(request.User?.GetUserDecksFromJwt().ToList() ?? new List<DeckType>(), request.BuyDeckDTO.DeckType)
                );
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("Not enough experience");
            }
            await _endpoint.Publish(new DeckBoughtDTO {
                DeckType = request.BuyDeckDTO.DeckType,
                UserId = Guid.Parse(request.User?.GetUserIdFromJwt() ?? "")
            }, cancellationToken);
        }

        public async Task Handle(BuyPartyCommand request, CancellationToken cancellationToken)
        {
            if (request.User == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            _validator = new ClaimValidator(RoleTypes.PartyExp, request.User?.GetUserExpFromJwt() ?? 0);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("Not enough experience");
            }
            await _endpoint.Publish(new PartyBoughtDTO
            {
                UserId = Guid.Parse(request.User?.GetUserIdFromJwt() ?? "")
            }, cancellationToken);
        }
    }
}
