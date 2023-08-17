using Hangfire;
using MediatR;

namespace game.api.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class MediatorExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="jobName"></param>
        /// <param name="request"></param>
        public static void Enqueue(this IMediator mediator, string jobName, IRequest request)
        {
            var client = new BackgroundJobClient();
            client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="request"></param>
        public static void Enqueue(this IMediator mediator, IRequest request)
        {
            var client = new BackgroundJobClient();
            client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(request));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="jobName"></param>
        /// <param name="request"></param>
        /// <param name="cron"></param>
        public static void AddOrUpdate(this IMediator mediator, string jobName, IRequest request, string cron)
        {
            RecurringJob.AddOrUpdate<MediatorHangfireBridge>(jobName, bridge => bridge.Send(jobName, request), cron);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="jobName"></param>
        /// <param name="request"></param>
        /// <param name="time"></param>
        public static void Schedule(this IMediator mediator, string jobName, IRequest request, DateTime time)
        {
            BackgroundJob.Schedule<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request), time);
        }
    }
}
