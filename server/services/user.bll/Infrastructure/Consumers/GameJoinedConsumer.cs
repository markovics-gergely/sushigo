using MassTransit;
using MediatR;
using shared.dal.Models;
using user.bll.Infrastructure.Commands;

namespace user.bll.Infrastructure.Consumers
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
            foreach (var user in context.Message.Users)
            {
                var command = new JoinGameCommand(new GameJoinedSingleDTO
                {
                    GameId = context.Message.GameId,
                    UserId = user.UserId,
                    PlayerId = user.PlayerId
                });
                await _mediator.Send(command, context.CancellationToken);

            }
        }
    }
}
