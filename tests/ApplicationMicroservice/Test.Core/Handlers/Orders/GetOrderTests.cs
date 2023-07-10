using Application.Contracts.Order.Queries;
using Application.Handlers.Orders;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;

namespace Test.Core.Handlers.Orders;

public class GetOrderTests : TestBase
{
    private readonly GetOrderHandler _handler;

    public GetOrderTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new GetOrderHandler(database.Context);
    }

    [Fact]
    public async Task Handle_Should_NotFind()
    {
        var query = new GetOrder.Query(Guid.NewGuid());

        var response = await _handler.Handle(query, CancellationToken.None);

        response.Order.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Find()
    {
        var order = await Database.Context.Orders.FirstAsync();
        var query = new GetOrder.Query(order.OrderId);

        var response = await _handler.Handle(query, CancellationToken.None);

        response.Order.Should().NotBeNull();
    }
}