using Application.Handlers.Extensions;
using Application.Middlewares.Extensions;
using Application.Validation.Extensions;
using Infrastructure.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Extensions;
using Presentation.WebAPI.Configuration;

namespace Presentation.WebAPI.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureServices(
        this IServiceCollection collection,
        IConfiguration configuration,
        WebApiConfiguration webApiConfiguration)
    {
        collection.AddDatabaseContext(o =>
            o.UseNpgsql(webApiConfiguration.PostgresConfiguration.ToConnectionString("ots"))
                .UseLazyLoadingProxies());

        collection.AddHealthChecks()
            .AddNpgSql(webApiConfiguration.PostgresConfiguration.ToConnectionString("ots"));

        collection.AddHandlers(configuration)
            .AddEndpoints(configuration)
            .AddValidation()
            .AddMiddlewares();

        return collection;
    }
}