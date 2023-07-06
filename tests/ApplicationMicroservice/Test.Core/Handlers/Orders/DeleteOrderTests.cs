using Application.Handlers.Orders;
using FluentAssertions;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;
using static Application.Contracts.Order.Commands.DeleteOrder;

namespace Test.Core.Handlers.Orders;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class DeleteOrderTests : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;
    private readonly DeleteOrderHandler _handler;

    public DeleteOrderTests(CoreDatabaseFixture database)
    {
        _database = database;
        _handler = new DeleteOrderHandler(_database.Context);
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
    public async Task DeleteOrder_ShouldNotThrow()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);
        var order = await _database.Context.Orders.FirstAsync();

        var command = new Command(order.OrderId);

        var response = await _handler.Handle(command, default);

        var result = response.Match(
            o => order.OrderId,
            err => Guid.Empty);

        result.Should().NotBe(Guid.Empty);
    }
}