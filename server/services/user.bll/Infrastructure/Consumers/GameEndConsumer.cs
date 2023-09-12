using MassTransit;
using MediatR;
using shared.dal.Models;
using user.bll.Infrastructure.Commands;

namespace user.bll.Infrastructure.Consumers
{
    public class GameEndConsumer : IConsumer<GameEndDTO>
    {
        private readonly IMediator _mediator;

        public GameEndConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<GameEndDTO> context)
        {
            var command = new EndGameCommand(context.Message);
            await _mediator.Send(command, context.CancellationToken);
        }
    }
}
