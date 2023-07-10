using Application.DataAccess.Contracts;
using Domain.Core.Contracts;
using Domain.Core.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.DatabaseContexts;

public sealed class DatabaseContext : DbContext, IDatabaseContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Person> People { get; set; } = null!;
    public DbSet<Courier> Couriers { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IAssemblyMarker).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}