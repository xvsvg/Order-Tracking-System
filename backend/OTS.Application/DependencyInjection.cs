using Microsoft.Extensions.DependencyInjection;

namespace OTS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));

        collection.AddAutoMapper(typeof(DependencyInjection).Assembly);

        return collection;
    }
}