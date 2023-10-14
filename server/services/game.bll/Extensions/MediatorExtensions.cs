using Hangfire;
using MediatR;

namespace game.bll.Extensions
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
        public static void Enqueue(this IMediator mediator, IBackgroundJobClient client, string jobName, IRequest request)
        {
            client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="jobName"></param>
        /// <param name="request"></param>
        public static void Enqueue(this IBackgroundJobClient client, string jobName, IRequest request)
        {
            client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="jobName"></param>
        public static void Dequeue(this IBackgroundJobClient client, string jobName)
        {
            client.Delete(jobName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="jobName"></param>
        /// <param name="request"></param>
        /// <param name="cron"></param>
        public static void AddOrUpdate(this IMediator mediator, IRecurringJobManager client, string jobName, IRequest request, string cron)
        {
            client.AddOrUpdate<MediatorHangfireBridge>(jobName, bridge => bridge.Send(jobName, request), cron);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="jobName"></param>
        /// <param name="request"></param>
        /// <param name="time"></param>
        public static void Schedule(this IMediator mediator, IBackgroundJobClient client, string jobName, IRequest request, DateTime time)
        {
            client.Schedule<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request), time);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="jobName"></param>
        /// <param name="request"></param>
        /// <param name="time"></param>
        public static void Schedule(this IBackgroundJobClient client, string jobName, IRequest request, DateTime time)
        {
            client.Schedule<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request), time);
        }

        public static void Delete(string jobName)
        {
            var monitor = JobStorage.Current.GetMonitoringApi();
            monitor.ProcessingJobs(0, int.MaxValue)
                .Where(x => x.Value.Job.Args[0].ToString() == jobName)
                .ToList()
                .ForEach(x => BackgroundJob.Delete(x.Key));
            monitor.ScheduledJobs(0, int.MaxValue)
                .Where(x => x.Value.Job.Args[0].ToString() == jobName)
                .ToList()
                .ForEach(x => BackgroundJob.Delete(x.Key));
        }
    }
}
