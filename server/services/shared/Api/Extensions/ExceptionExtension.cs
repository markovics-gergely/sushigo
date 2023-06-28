using Hellang.Middleware.ProblemDetails;
using Microsoft.Extensions.DependencyInjection;
using shared.Logic.Exceptions;

namespace shared.Api.Extensions
{
    /// <summary>
    /// Manage custom exceptions
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// Add problem details configurations
        /// </summary>
        /// <param name="services"></param>
        public static void AddExceptionExtensions(this IServiceCollection services)
        {
            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, ex) => false;
                options.Map<EntityNotFoundException>(
                (ctx, ex) =>
                {
                    var pd = StatusCodeProblemDetails.Create(StatusCodes.Status404NotFound);
                    pd.Title = ex.Message;
                    return pd;
                });
                options.Map<InvalidParameterException>(
                (ctx, ex) =>
                {
                    var pd = StatusCodeProblemDetails.Create(StatusCodes.Status400BadRequest);
                    pd.Title = ex.Message;
                    return pd;
                });
                options.Map<ValidationErrorException>(
                (ctx, ex) =>
                {
                    var pd = StatusCodeProblemDetails.Create(StatusCodes.Status400BadRequest);
                    pd.Title = ex.Message;
                    return pd;
                });
            });
        }
    }
}
