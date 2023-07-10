using Application.DataAccess.Contracts;
using Domain.Core.Implementations;
using Infrastructure.DataAccess.DatabaseContexts;
using Infrastructure.Seeding.Generators;

namespace Infrastructure.Seeding.Helpers;

public static class SeedingHelper
{
    public static async Task SeedDatabaseAsync(IDatabaseContext context)
    {
        if (context is DatabaseContext ctx)
            await ctx.Database.EnsureCreatedAsync();

        await context.Couriers.AddRangeAsync(EntityGenerator<Courier>.Generate(100));
        await context.Orders.AddRangeAsync(ModifyCustomerOrdersToUtc(EntityGenerator<Order>.Generate(100)));
        await context.Customers.AddRangeAsync(EntityGenerator<Customer>.Generate(100));

        await context.SaveChangesAsync(default);
    }

    private static IEnumerable<Order> ModifyCustomerOrdersToUtc(IEnumerable<Order> orders)
    {
        var result = new List<Order>();

        foreach (var order in orders)
        {
            var modifiedOrders = order.Customer.OrderHistory.Select(o => new Order(
                Guid.NewGuid(),
                o.DispatchDate.ToUniversalTime(),
                o.DeliveryDate?.ToUniversalTime() ?? o.DeliveryDate,
                o.Courier,
                o.Customer,
                o.Name)).ToList();

            var customer = new Customer(
                Guid.NewGuid(),
                order.Customer.FullName,
                order.Customer.ContactInfo.ToArray());

            modifiedOrders.ForEach(o => o.Customer.AddOrderToHistory(o));

            var newOrder = new Order(
                Guid.NewGuid(),
                order.DispatchDate.ToUniversalTime(),
                order.DeliveryDate?.ToUniversalTime() ?? order.DeliveryDate,
                order.Courier,
                customer,
                order.Name);

            result.Add(newOrder);
        }

        return result;
    }
}