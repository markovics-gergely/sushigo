using System.ComponentModel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace game.bll.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class MediatorHangfireBridge
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MediatorHangfireBridge> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public MediatorHangfireBridge(IMediator mediator, ILogger<MediatorHangfireBridge> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private async Task Send(IRequest command)
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
            _logger.LogInformation($"Executing job {jobName}");
            await _mediator.Send(command);
        }
    }
}
