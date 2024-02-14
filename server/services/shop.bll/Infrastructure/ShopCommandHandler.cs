using MassTransit;
using MediatR;
using shared.dal.Models;
using shop.bll.Infrastructure.Commands;
using shared.bll.Validators.Implementations;
using shared.bll.Validators.Interfaces;
using shop.bll.Validators;
using shared.bll.Exceptions;
using shared.bll.Extensions;
using shared.dal.Models.Types;

namespace shop.bll.Infrastructure
{
    public class ShopCommandHandler :
        IRequestHandler<BuyDeckCommand>,
        IRequestHandler<BuyPartyCommand>
    {
        private readonly IPublishEndpoint _endpoint;
        private IValidator? _validator;

        public ShopCommandHandler(IPublishEndpoint endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task Handle(BuyDeckCommand request, CancellationToken cancellationToken)
        {
            var guid = Guid.Parse(request.User?.GetUserIdFromJwt() ?? "");

            // Validate right to buy the deck
            _validator = new AndCondition(
                    new ClaimValidator(RoleTypes.DeckExp, request.User?.GetUserExpFromJwt() ?? 0),
                    new ClaimDeckValidator(request.User?.GetUserDecksFromJwt().ToList() ?? new List<DeckType>(), request.BuyDeckDTO.DeckType)
                );
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(BuyDeckCommand));
            }

            // Send message to buy the deck to RabbitMQ
            await _endpoint.Publish(new DeckBoughtDTO {
                DeckType = request.BuyDeckDTO.DeckType,
                UserId = guid
            }, cancellationToken);
        }

        public async Task Handle(BuyPartyCommand request, CancellationToken cancellationToken)
        {
            var guid = Guid.Parse(request.User?.GetUserIdFromJwt() ?? "");

            // Validate right to buy the party mode
            _validator = new ClaimValidator(RoleTypes.PartyExp, request.User?.GetUserExpFromJwt() ?? 0);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException(nameof(BuyPartyCommand));
            }

            // Send message to buy the party mode to RabbitMQ
            await _endpoint.Publish(new PartyBoughtDTO
            {
                UserId = Guid.Parse(request.User?.GetUserIdFromJwt() ?? "")
            }, cancellationToken);
        }
    }
}
