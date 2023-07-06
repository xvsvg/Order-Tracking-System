using Application.Handlers.Customers;
using FluentAssertions;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;
using static Application.Contracts.Customer.Commands.RemoveOrder;

namespace Test.Core.Handlers.Customers;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class RemoveOrderTests : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;
    private readonly RemoveOrderHandler _handler;

    public RemoveOrderTests(CoreDatabaseFixture database)
    {
        _database = database;
        _handler = new RemoveOrderHandler(_database.Context);
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
    public async Task RemoveOrder_ShouldThrow_IfOrderExists()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);
        var customer = await _database.Context.Customers
            .FirstOrDefaultAsync(x => x.OrderHistory.Any());

        var order = customer!.OrderHistory.First();

        var command = new Command(customer.PersonId, order.OrderId);

        var response = await _handler.Handle(command, default);

        var act = async () =>
        {
            return await _database.Context.Orders.FirstAsync(x => x.OrderId.Equals(order.OrderId));
        };

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}