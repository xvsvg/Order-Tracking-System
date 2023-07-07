using Application.Handlers.Orders;
using FluentAssertions;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;
using static Application.Contracts.Order.Commands.DeleteOrder;

namespace Test.Core.Handlers.Orders;

public class DeleteOrderTests : TestBase
{
    private readonly DeleteOrderHandler _handler;

    public DeleteOrderTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new DeleteOrderHandler(database.Context);
    }

    [Fact]
    public async Task DeleteOrder_Should_SuccessfullyDelete()
    {
        var order = await Database.Context.Orders.FirstAsync();
        var command = new Command(order.OrderId);

        var response = await _handler.Handle(command, default);

        var result = response.Match(
            o => order.OrderId,
            err => Guid.Empty);

        result.Should().NotBe(Guid.Empty);
    }
}