using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OTS.Application.Contracts.Database;
using OTS.Persistence.DatabaseContexts;

namespace OTS.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection collection,
        Action<DbContextOptionsBuilder> configuration)
    {
        collection.AddDbContext<IOtsDatabaseContext, OtsDatabaseContext>(configuration);

        return collection;
    }
}