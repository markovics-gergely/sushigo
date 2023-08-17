using System.ComponentModel;
using MediatR;

namespace game.api.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class MediatorHangfireBridge
    {
        private readonly IMediator _mediator;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public MediatorHangfireBridge(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task Send(IRequest command)
        {
            await _mediator.Send(command);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [DisplayName("{0}")]
        public async Task Send(string jobName, IRequest command)
        {
            await _mediator.Send(command);
        }
    }
}
