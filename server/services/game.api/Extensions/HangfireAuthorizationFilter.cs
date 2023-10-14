using Hangfire.Dashboard;

namespace game.api.Extensions
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            return true;
            //return httpContext.User.Identity?.IsAuthenticated ?? false;
        }
    }
}
