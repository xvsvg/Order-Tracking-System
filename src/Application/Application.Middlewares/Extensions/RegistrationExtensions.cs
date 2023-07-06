namespace Application.Middlewares.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddMiddlewares(this IServiceCollection collection)
    {
        collection.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<IAssemblyMarker>());

        collection
            .AddTransient<DatabaseErrorMappingMiddleware>()
            .AddTransient<ValidationMappingMiddleware>();

        return collection;
    }
}