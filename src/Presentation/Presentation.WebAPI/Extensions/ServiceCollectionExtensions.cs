using Application.Handlers.Extensions;
using Application.Middlewares.Extensions;
using Application.Validation.Extensions;
using Infrastructure.DataAccess.Configuration;
using Infrastructure.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Extensions;
using Presentation.WebAPI.Configuration;

namespace Presentation.WebAPI.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        var postgresConfiguration = configuration
            .GetSection("PostgresConfiguration")
            .Get<PostgresConfiguration>()
            ?? throw new Exception("Unable to define database configuration.");
        
        collection.AddDatabaseContext(o =>
            o.UseNpgsql(postgresConfiguration.ToConnectionString("ots"))
                .UseLazyLoadingProxies());

        collection.AddHealthChecks()
            .AddNpgSql(postgresConfiguration.ToConnectionString("ots"));

        collection.AddHandlers(configuration)
            .AddEndpoints(configuration)
            .AddValidation()
            .AddMiddlewares();

        return collection;
    }
}