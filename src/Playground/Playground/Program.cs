using Domain.Core.Implementations;
using Domain.Core.ValueObjects;
using Infrastructure.DataAccess.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

await CreateDatabaseAsync();

async Task CreateDatabaseAsync()
{
    DbContextOptionsBuilder<DatabaseContext> builder = new DbContextOptionsBuilder<DatabaseContext>()
        .UseNpgsql("Host=localhost;Port=1433;Database=ots;Username=postgres;Password=postgres")
        .UseLazyLoadingProxies();

    var context = new DatabaseContext(builder.Options);
    
    await context.Database.EnsureCreatedAsync();
    
    var customer = new Customer(new FullName("asd", "sad", "asd"), new ContactInfo("email"));
    var courier = new Courier(new FullName("bew", "new", "ewa"), new ContactInfo("phone"));
    var order = new Order(DateTime.UtcNow, DateTime.UtcNow.AddDays(1), null, customer, "whatever");
    
    customer.AddOrderToHistory(order);
    
    context.Customers.Add(customer);
    context.Couriers.Add(courier);
    context.Orders.Add(order);

    // var courier = await context.Couriers.FirstAsync();
    // var order = await context.Orders.FirstAsync();
    //
    order.Courier = courier;

    await context.SaveChangesAsync();
}