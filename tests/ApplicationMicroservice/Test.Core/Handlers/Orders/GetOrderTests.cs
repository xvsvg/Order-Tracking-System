using Application.Handlers.Orders;
using FluentAssertions;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;
using static Application.Contracts.Order.Queries.GetOrder;

namespace Test.Core.Handlers.Orders;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class GetOrderTests : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;

    public GetOrderTests(CoreDatabaseFixture database)
    {
        _database = database;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _database.ResetAsync();
    }

    [Fact]
    public async Task Handle_Should_NotFind()
    {
        var query = new Query(Guid.NewGuid());
        var handler = new GetOrderHandler(_database.Context);

        var response = await handler.Handle(query, CancellationToken.None);

        response.Order.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_NotThrow()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);

        var order = await _database.Context.Orders.FirstAsync();
        var query = new Query(order.OrderId);
        var handler = new GetOrderHandler(_database.Context);

        var response = await handler.Handle(query, CancellationToken.None);

        response.Order.Should().NotBeNull();
    }
}