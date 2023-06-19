using Application.DataAccess.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataAccess.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddDatabaseContext(
        this IServiceCollection collection,
        Action<DbContextOptionsBuilder> configuration)
    {
        collection.AddDbContext<IDatabaseContext, DatabaseContext.DatabaseContext>(configuration);

        return collection;
    }

    public static Task UseDatabaseContext(this IServiceScope scope)
    {
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext.DatabaseContext>();
        return context.Database.MigrateAsync();
    }
}