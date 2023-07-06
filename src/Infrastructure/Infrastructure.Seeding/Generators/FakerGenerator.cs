using AutoBogus;
using Bogus;
using Domain.Core.Implementations;
using Domain.Core.ValueObjects;

namespace Infrastructure.Seeding.Generators;

public class FakerGenerator
{
    private static readonly Faker<Customer>? CustomerFaker;
    private static readonly Faker<Courier>? CourierFaker;
    private static readonly Faker<Order>? OrderFaker;

    static FakerGenerator()
    {
        CustomerFaker = new AutoFaker<Customer>()
            .RuleFor(c => c.PersonId, f => Guid.NewGuid())
            .RuleFor(c => c.FullName, f => new FullName(f.Name.FirstName(), f.Name.LastName(), f.Name.FullName()))
            .RuleFor(c => c.OrderHistory, f => Array.Empty<Order>().ToList())
            .RuleFor(c => c.ContactInfo, f => new List<ContactInfo> { new ContactInfo(f.Phone.PhoneNumber()) });


        OrderFaker = new AutoFaker<Order>()
            .RuleFor(o => o.Name, f => f.Random.Chars(count: 10).ToString())
            .RuleFor(o => o.OrderId, f => Guid.NewGuid())
            .RuleFor(o => o.Courier, f => null)
            .RuleFor(o => o.DeliveryDate, f => f.Date.Recent(0).ToUniversalTime())
            .RuleFor(o => o.DispatchDate, f => f.Date.Recent(0).ToUniversalTime());

        CourierFaker = new AutoFaker<Courier>()
            .RuleFor(c => c.PersonId, f => Guid.NewGuid())
            .RuleFor(c => c.FullName, f => new FullName(f.Name.FirstName(), f.Name.LastName(), f.Name.FullName()))
            .RuleFor(c => c.ContactInfo, f => new List<ContactInfo> { new ContactInfo(f.Phone.PhoneNumber()) })
            .RuleFor(c => c.DeliveryList, f => Array.Empty<Order>().ToList());
    }

    public static Faker<T> GeneratorOfType<T>() where T : class
    {
        return typeof(T) switch
        {
            { } courierType when courierType == typeof(Courier) => CourierFaker as Faker<T>,
            { } customerType when customerType == typeof(Customer) => CustomerFaker as Faker<T>,
            { } orderType when orderType == typeof(Order) => OrderFaker as Faker<T>,
            _ => throw new ArgumentException("Unsupported type"),
        } ?? throw new ArgumentException("Unsupported type");
    }
}