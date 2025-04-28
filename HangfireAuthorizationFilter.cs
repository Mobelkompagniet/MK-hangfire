using Hangfire.Dashboard;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    private readonly string[] _roles;

    public HangfireAuthorizationFilter(params string[] roles)
    {
        _roles = roles;
    }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = ((AspNetCoreDashboardContext)context).HttpContext;

        // Authorization logic goes here.
        // NOT IMPLEMENTED - handled by Nginx

        return true;
    }
}
