using Application.Handlers.Customers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Test.Tools.Helpers;
using Xunit;
using static Application.Contracts.Customer.Queries.GetCustomer;

namespace Test.Core.Handlers.Couriers;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class GetCourierTests : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;
    private readonly GetCustomerHandler _handler;

    public GetCourierTests(CoreDatabaseFixture database)
    {
        _database = database;
        _handler = new GetCustomerHandler(_database.Context);
    }

    [Fact]
    public async Task GetCustomer_ShouldFind()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);
        var customer = await _database.Context.Customers.FirstAsync();

        var query = new Query(customer.PersonId);

        var response = await _handler.Handle(query, default);

        response.Customer.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetCustomer_ShouldNotFind()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);

        var query = new Query(Guid.NewGuid());

        var response = await _handler.Handle(query, default);

        response.Customer.Should().BeNull();
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