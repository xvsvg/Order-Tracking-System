using Application.Contracts.Order.Commands;
using Application.Handlers.Orders;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;

namespace Test.Core.Handlers.Orders;

public class UpdateOrderTests : TestBase
{
    private readonly UpdateOrderHandler _handler;

    public UpdateOrderTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new UpdateOrderHandler(database.Context);
    }

    [Fact]
    public async Task UpdateOrder_ShouldNotThrow()
    {
        var order = await Database.Context.Orders.FirstAsync();
        var command = new UpdateOrder.Command(
            order.OrderId,
            "New name",
            order.DispatchDate,
            order.DeliveryDate,
            order.Courier?.PersonId,
            order.Customer.PersonId);
        Database.Context.Entry(order).State = EntityState.Detached;

        var response = await _handler.Handle(command, default);

        var result = response.Match(
            x => x.Order,
            x => null!);

        result.Should().NotBeNull();
        result.Name.Should().Be("New name");
    }
}