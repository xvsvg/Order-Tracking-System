using Application.Contracts.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Application.Handlers.Extensions;

public static class RegistrationExtensions
{
    private const int DefaultPageItemCount = 1;

    public static IServiceCollection AddHandlers(this IServiceCollection collection, IConfiguration configuration)
    {
        var paginationConfiguration = configuration.GetSection("Pagination").Get<PaginationConfiguration>()
                                      ?? new PaginationConfiguration(DefaultPageItemCount);

        collection.TryAddSingleton(paginationConfiguration);

        collection.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<IAssemblyMarker>());
        return collection;
    }
}