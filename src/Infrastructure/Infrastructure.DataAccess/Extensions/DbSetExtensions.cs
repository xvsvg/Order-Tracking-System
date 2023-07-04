using Domain.Core.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Extensions;

public static class DbSetExtensions
{
    public static async Task<Customer?> FindExistingCustomerAsync(
        this DbSet<Customer> customers,
        Customer customer,
        CancellationToken cancellationToken)
    {
        var matchingCustomer = await customers.FirstOrDefaultAsync(x =>
            x.FullName.Equals(customer.FullName), cancellationToken);

        if (matchingCustomer != null &&
            matchingCustomer.ContactInfo.Count() == customer.ContactInfo.Count() &&
            matchingCustomer.ContactInfo.OrderBy(ci => ci).SequenceEqual(customer.ContactInfo.OrderBy(ci => ci)))
        {
            return matchingCustomer;
        }

        return null;
    }
}