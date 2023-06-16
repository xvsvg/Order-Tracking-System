using Domain.Core.Contracts;
using Domain.Core.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Application.DataAccess.Contracts;

public interface IDatabaseContext
{
    DbSet<Order> Orders { get; set; }
    DbSet<Person> People { get; set; }
    DbSet<Courier> Couriers { get; set; }
    DbSet<Customer> Customers { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}