using Application.Contracts.Order.Commands;
using Application.Handlers.Orders;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;

namespace Test.Core.Handlers.Orders;

public class CreateOrderTests : TestBase
{
    private readonly CreateOrderHandler _handler;

    public CreateOrderTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new CreateOrderHandler(database.Context);
    }

    [Fact]
    public async Task CreateValidOrder_Should_PassValidation_And_BeCreated()
    {
        var customer = await Database.Context.Customers.FirstAsync();
        var courier = await Database.Context.Couriers.FirstAsync();
        var command = new CreateOrder.Command(
            "Whatever",
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(2),
            courier.PersonId,
            customer.PersonId);

        var response = await _handler.Handle(command, default);

        var result = response.Match(
            x => x.Order,
            x => null!);

        result.Should().NotBeNull();
        result.Name.Should().Be("Whatever");
        result.CourierId.Should().Be(courier.PersonId);
        result.CustomerId.Should().Be(customer.PersonId);
    }

    [Fact]
    public async Task CreateInvalidOrder_Should_NotPassValidation()
    {
        var command = new CreateOrder.Command(
            "123asdas",
            DateTime.MinValue,
            null,
            null,
            Guid.Empty);

        var response = await _handler.Handle(command, default);

        var result = response.Match(
            x => x.Order,
            x => null!);

        result.Should().BeNull();
    }
}