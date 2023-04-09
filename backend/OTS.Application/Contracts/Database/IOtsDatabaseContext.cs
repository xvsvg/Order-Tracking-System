using Microsoft.EntityFrameworkCore;
using OTS.Domain.Domain.Core.Contracts;
using OTS.Domain.Domain.Core.Implementations;

namespace OTS.Application.Contracts.Database;

public interface IOtsDatabaseContext
{
    DbSet<Order> Orders { get; set; }
    DbSet<Person> People { get; set; }
    DbSet<Courier> Couriers { get; set; }
    DbSet<Customer> Customers { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}