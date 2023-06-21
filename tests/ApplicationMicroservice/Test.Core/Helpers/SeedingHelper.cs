using Application.DataAccess.Contracts;
using AutoBogus;
using Bogus;
using Domain.Core.Implementations;
using Domain.Core.ValueObjects;
using Infrastructure.DataAccess.DatabaseContext;

namespace Test.Core.Helpers;

public static class SeedingHelper
{
    private static readonly Faker<Customer>? _customerFaker;
    private static readonly Faker<Courier>? _courierFaker;
    private static readonly Faker<Order>? _orderFaker;
    
    static SeedingHelper()
    {
        _customerFaker = new AutoFaker<Customer>()
            .RuleFor(c => c.PersonId, f => Guid.NewGuid())
            .RuleFor(c => c.FullName, f => new FullName(f.Name.FirstName(), f.Name.LastName(), f.Name.FullName()))
            .RuleFor(c => c.OrderHistory, f => new List<Order>())
            .RuleFor(c => c.ContactInfo, f => new List<ContactInfo>{ new ContactInfo(f.Phone.PhoneNumber()) });


        _orderFaker = new AutoFaker<Order>()
            .RuleFor(o => o.Name, f => f.Random.Chars(count: 10).ToString())
            .RuleFor(o => o.OrderId, f => Guid.NewGuid())
            .RuleFor(o => o.Courier, f => null)
            .RuleFor(o => o.DeliveryDate, f => f.Date.Recent(0).ToUniversalTime())
            .RuleFor(o => o.DispatchDate, f => f.Date.Recent(0).ToUniversalTime())
            .RuleFor(o => o.Customer, (f, o) =>
            {
                var customer = _customerFaker.Generate(1).Single();
                customer.AddOrderToHistory(o);
                return customer;
            });
        
        _courierFaker = new AutoFaker<Courier>()
            .RuleFor(c => c.PersonId, f => Guid.NewGuid())
            .RuleFor(c => c.FullName, f => new FullName(f.Name.FirstName(), f.Name.LastName(), f.Name.FullName()))
            .RuleFor(c => c.ContactInfo, f => new List<ContactInfo> { new ContactInfo(f.Phone.PhoneNumber()) })
            .RuleFor(c => c.DeliveryList, (f, c) =>
            {
                var orders = _orderFaker.Generate(5);
        
                orders.Select(c.AddOrderToDeliver);
                
                return orders;
            });
    }

    public static async Task SeedDatabaseAsync(IDatabaseContext context)
    {
        if (context is DatabaseContext ctx)
            await ctx.Database.EnsureCreatedAsync();
        
        await context.Couriers.AddRangeAsync(_courierFaker!.Generate(100));
        await context.Orders.AddRangeAsync(ChangeToUtc(_orderFaker!.Generate(100)));
        await context.Customers.AddRangeAsync(_customerFaker!.Generate(100));

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