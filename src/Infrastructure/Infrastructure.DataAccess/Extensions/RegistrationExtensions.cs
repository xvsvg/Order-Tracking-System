using Application.DataAccess.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataAccess.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection collection,
        Action<DbContextOptionsBuilder> configuration)
    {
        collection.AddDbContext<IDatabaseContext, DatabaseContext.DatabaseContext>(configuration);

        return collection;
    }
}