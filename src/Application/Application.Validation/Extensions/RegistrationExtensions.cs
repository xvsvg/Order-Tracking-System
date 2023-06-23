using FluentValidation;

namespace Application.Validation.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddValidation(this IServiceCollection collection)
    {
        collection.AddInternalValidatorsFromAssemblyContaining<IAssemblyMarker>();
        collection.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<IAssemblyMarker>());
        return collection;
    }

    private static void AddInternalValidatorsFromAssemblyContaining<T>(this IServiceCollection collection)
    {
        var types = typeof(T).Assembly.GetTypes();
        new AssemblyScanner(types).ForEach(pair =>
        {
            collection.Add(ServiceDescriptor.Transient(pair.InterfaceType, pair.ValidatorType));
        });
    }
}