using Application.Middlewares;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Presentation.Endpoints.Extensions;

namespace Presentation.WebAPI.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.UseMiddleware<DatabaseErrorMappingMiddleware>()
            .UseMiddleware<ValidationMappingMiddleware>();

        app.UseEndpoints();

        app.MapHealthChecks("_health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}