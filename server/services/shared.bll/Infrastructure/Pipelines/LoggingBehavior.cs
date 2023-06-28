using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace shared.bll.Infrastructure.Pipelines
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        public LoggingBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = request.GetType().Name;
            var requestGuid = Guid.NewGuid().ToString();

            _logger.LogInformation("[START] {requestName} [{requestGuid}]", requestName, requestGuid);
            TResponse response;

            var stopwatch = Stopwatch.StartNew();
            try
            {
                /*try
                {
                    _logger.LogInformation(
                        "[PROPS] {requestName} [{requestGuid}] {data}",
                        requestName, requestGuid, JsonSerializer.Serialize(request)
                    );
                }
                catch (NotSupportedException)
                {
                    _logger.LogInformation(
                        "[Serialization ERROR] {requestName} [{requestGuid}] Could not serialize the request.",
                        requestName, requestGuid
                    );
                }*/
                response = await next();
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation(
                    "[END] {requestName} [{requestGuid}]; Execution time={elapsed}ms",
                    requestName, requestGuid, stopwatch.ElapsedMilliseconds
                );
            }

            return response;
        }
    }
}
