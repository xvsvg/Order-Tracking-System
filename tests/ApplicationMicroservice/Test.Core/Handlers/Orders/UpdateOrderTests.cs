using Application.Handlers.Orders;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Test.Tools.Helpers;
using Xunit;
using static Application.Contracts.Order.Commands.UpdateOrder;

namespace Test.Core.Handlers.Orders;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class UpdateOrderTests : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;
    private readonly UpdateOrderHandler _handler;

    public UpdateOrderTests(CoreDatabaseFixture database)
    {
        _database = database;
        _handler = new UpdateOrderHandler(_database.Context);
    }

    [Fact]
    public async Task UpdateOrder_ShouldNotThrow()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);

        var order = await _database.Context.Orders.FirstAsync();

        var command = new Command(
            order.OrderId,
            "New name",
            order.DispatchDate,
            order.DeliveryDate,
            order.Courier?.PersonId,
            order.Customer.PersonId);
        
        _database.Context.Entry(order).State = EntityState.Detached;

        var response = await _handler.Handle(command, default);

        var result = response.Match(
            x => x.Order,
            x => null!);

        result.Should().NotBeNull();
        result.Name.Should().Be("New name");
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _database.ResetAsync();
    }
}