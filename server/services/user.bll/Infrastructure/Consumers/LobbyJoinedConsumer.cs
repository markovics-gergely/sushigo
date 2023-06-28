using MassTransit;
using MediatR;
using shared.dal.Models;
using user.bll.Infrastructure.Commands;

namespace user.bll.Infrastructure.Consumers
{
    public class LobbyJoinedConsumer : IConsumer<LobbyJoinedDTO>
    {
        private readonly IMediator _mediator;

        public LobbyJoinedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<LobbyJoinedDTO> context)
        {
            var command = new JoinLobbyCommand(context.Message);
            await _mediator.Send(command, context.CancellationToken);
        }
    }
}
