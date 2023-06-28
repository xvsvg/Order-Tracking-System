using Application.Handlers.Orders;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Test.Tools.Helpers;
using Xunit;
using static Application.Contracts.Order.Commands.CreateOrder;

namespace Test.Core.Handlers.Orders;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class CreateOrderTest : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;
    private readonly CreateOrderHandler _handler;

    public CreateOrderTest(CoreDatabaseFixture database)
    {
        _database = database;
        _handler = new CreateOrderHandler(_database.Context);
    }

    [Fact]
    public async Task CreateValidOrder_ShouldPassValidation_And_BeCreated()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);
        var customer = await _database.Context.Customers.FirstAsync();
        var courier = await _database.Context.Couriers.FirstAsync();

        var command = new Command(
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
    public async Task CreateInvalidOrder_ShouldNotPassValidation()
    {
        var command = new Command(
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

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _database.ResetAsync();
    }
}