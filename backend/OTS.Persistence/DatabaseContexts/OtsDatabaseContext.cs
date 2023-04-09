using Microsoft.EntityFrameworkCore;
using OTS.Application.Contracts.Database;
using OTS.Domain.Domain.Core.Contracts;
using OTS.Domain.Domain.Core.Implementations;

namespace OTS.Persistence.DatabaseContexts;

public sealed class OtsDatabaseContext : DbContext, IOtsDatabaseContext
{
    public OtsDatabaseContext(DbContextOptions<OtsDatabaseContext> options)
        : base(options) { }

    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Person> People { get; set; } = null!;
    public DbSet<Courier> Couriers { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OtsDatabaseContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}