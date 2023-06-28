using MassTransit;
using MediatR;
using shared.dal.Models;
using user.bll.Infrastructure.Commands;

namespace user.bll.Infrastructure.Consumers
{
    public class DeckBoughtConsumer : IConsumer<DeckBoughtDTO>, IConsumer<PartyBoughtDTO>
    {
        private readonly IMediator _mediator;

        public DeckBoughtConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<DeckBoughtDTO> context)
        {
            var command = new ClaimDeckCommand(context.Message);
            await _mediator.Send(command, context.CancellationToken);
        }

        public async Task Consume(ConsumeContext<PartyBoughtDTO> context)
        {
            var command = new ClaimPartyCommand(context.Message);
            await _mediator.Send(command, context.CancellationToken);
        }
    }
}
