using Application.DataAccess.Contracts;
using Domain.Core.Implementations;
using Infrastructure.DataAccess.DatabaseContexts;
using Test.Tools.Generators;

namespace Test.Tools.Helpers;

public static class SeedingHelper
{
    public static async Task SeedDatabaseAsync(IDatabaseContext context)
    {
        if (context is DatabaseContext ctx)
            await ctx.Database.EnsureCreatedAsync();

        await context.Couriers.AddRangeAsync(EntityGenerator<Courier>.Generate(100));
        await context.Orders.AddRangeAsync(ChangeToUtc(EntityGenerator<Order>.Generate(100)));
        await context.Customers.AddRangeAsync(EntityGenerator<Customer>.Generate(100));

        await context.SaveChangesAsync(default);
    }

    private static IEnumerable<Order> ChangeToUtc(IEnumerable<Order> orders)
    {
        return orders.Select(x => new Order(
            dispatchDate: DateTime.UtcNow,
            deliveryDate: x.DeliveryDate,
            courier: x.Courier,
            customer: x.Customer,
            name: x.Name
        ));
    }
}