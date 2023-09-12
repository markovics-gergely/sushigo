using MassTransit;
using MediatR;
using shared.dal.Models;
using user.bll.Infrastructure.Commands;

namespace user.bll.Infrastructure.Consumers
{
    public class GameJoinedConsumer : IConsumer<GameJoinedDTO>
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _endpoint;

        public GameJoinedConsumer(IMediator mediator, IPublishEndpoint endpoint)
        {
            _mediator = mediator;
            _endpoint = endpoint;
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
            await _endpoint.Publish(new LobbyRemoveDTO { LobbyId = context.Message.LobbyId });
        }
    }
}
