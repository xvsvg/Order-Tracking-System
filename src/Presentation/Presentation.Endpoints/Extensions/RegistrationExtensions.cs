using Application.Dto;
using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Presentation.Endpoints.Tools;

namespace Presentation.Endpoints.Extensions;

public static class RegistrationExtensions
{
    private const int DefaultTimeInSeconds = 10;

    public static IServiceCollection AddEndpoints(this IServiceCollection collection, IConfiguration configuration)
    {
        var cacheConfiguration = configuration.GetSection("Cache").Get<CacheConfiguration>()
                                 ?? new CacheConfiguration(DefaultTimeInSeconds);

        collection.TryAddSingleton(cacheConfiguration);
        return collection.AddFastEndpoints().AddResponseCaching();
    }

    public static WebApplication UseEndpoints(this WebApplication app)
    {
        app.UseResponseCaching().UseFastEndpoints(x =>
        {
            x.Errors.ResponseBuilder = (failures, _, _) =>
            {
                return new ValidationFailureDto(failures.ToDictionary(x => x.PropertyName,
                    x => x.ErrorMessage));
            };
        });

        return app;
    }
}