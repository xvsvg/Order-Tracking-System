using Application.Handlers.Customers;
using FluentAssertions;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;
using static Application.Contracts.Customer.Queries.GetCustomer;

namespace Test.Core.Handlers.Customers;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class GetCustomerTests : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;
    private readonly GetCustomerHandler _handler;

    public GetCustomerTests(CoreDatabaseFixture database)
    {
        _database = database;
        _handler = new GetCustomerHandler(_database.Context);
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
    public async Task GetCustomer_ShouldNotFind()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);
        var query = new Query(Guid.NewGuid());

        var response = await _handler.Handle(query, default);

        response.Should().NotBeNull();
        response!.Customer.Should().BeNull();
    }

    [Fact]
    public async Task GetCustomer_ShouldFind()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);
        var customer = await _database.Context.Customers.FirstAsync();
        var query = new Query(customer.PersonId);

        var response = await _handler.Handle(query, default);

        response.Should().NotBeNull();
        response!.Customer.Should().NotBeNull();
        response!.Customer!.Id.Should().Be(customer.PersonId);
    }
}