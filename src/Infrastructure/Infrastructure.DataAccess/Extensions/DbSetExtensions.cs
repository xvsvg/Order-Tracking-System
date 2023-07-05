using Domain.Core.Contracts;
using Domain.Core.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Extensions;

public static class DbSetExtensions
{
    public static async Task<T?> FindExistingPersonAsync<T>(
        this DbSet<T> people,
        T customer,
        CancellationToken cancellationToken) where T : Person
    {
        var matching = await people.FirstOrDefaultAsync(x =>
            x.FullName.Equals(customer.FullName), cancellationToken);

        if (matching != null &&
            matching.ContactInfo.Count() == customer.ContactInfo.Count() &&
            matching.ContactInfo.OrderBy(ci => ci).SequenceEqual(customer.ContactInfo.OrderBy(ci => ci)))
        {
            return matching;
        }

        return null;
    }
}