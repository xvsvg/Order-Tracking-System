using Application.DataAccess.Contracts;
using Application.Handlers.Extensions;
using Application.Middlewares;
using Application.Middlewares.Extensions;
using Application.Validation.Extensions;
using HealthChecks.UI.Client;
using Infrastructure.DataAccess.Extensions;
using Infrastructure.Seeding.Helpers;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Extensions;

namespace Playground.Web;

public partial class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDatabaseContext(o => o
            .UseNpgsql("Host=localhost;Port=1433;Database=ots;Username=postgres;Password=postgres")
            .UseLazyLoadingProxies());

        builder.Services.AddHealthChecks()
            .AddNpgSql("Host=localhost;Port=1433;Database=ots;Username=postgres;Password=postgres");
    
        builder.Services.AddHandlers(builder.Configuration);
        builder.Services.AddEndpoints(builder.Configuration);
        builder.Services.AddValidation();
        builder.Services.AddMiddlewares();

        var app = builder.Build();

        app.UseMiddleware<DatabaseErrorMappingMiddleware>();

        await using (var scope = app.Services.CreateAsyncScope())
        {
            await SeedingHelper.SeedDatabaseAsync(scope.ServiceProvider.GetRequiredService<IDatabaseContext>());
        }
        app.UseMiddleware<ValidationMappingMiddleware>();
        
        app.MapHealthChecks("_health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.UseEndpoints();
        await app.RunAsync();
    }
}