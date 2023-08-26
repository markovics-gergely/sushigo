using lobby.bll.Infrastructure.Commands;
using MassTransit;
using MediatR;
using shared.dal.Models;

namespace lobby.bll.Infrastructure.Consumers
{
    public class GameJoinedConsumer : IConsumer<GameJoinedDTO>
    {
        private readonly IMediator _mediator;

        public GameJoinedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<GameJoinedDTO> context)
        {
            if (!string.IsNullOrEmpty(context.Message.LobbyId.ToString()))
            {
                var command = new RemoveLobbyCommand(context.Message.LobbyId);
                await _mediator.Send(command, context.CancellationToken);
            }
        }
    }
}
