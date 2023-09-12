using lobby.bll.Infrastructure.Commands;
using MassTransit;
using MediatR;
using shared.dal.Models;

namespace lobby.bll.Infrastructure.Consumers
{
    public class LobbyRemoveConsumer : IConsumer<LobbyRemoveDTO>
    {
        private readonly IMediator _mediator;

        public LobbyRemoveConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<LobbyRemoveDTO> context)
        {
            if (!string.IsNullOrEmpty(context.Message.LobbyId.ToString()))
            {
                var command = new RemoveLobbyCommand(context.Message.LobbyId);
                await _mediator.Send(command, context.CancellationToken);
            }
        }
    }
}
