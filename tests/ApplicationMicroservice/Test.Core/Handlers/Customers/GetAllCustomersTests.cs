using Application.Contracts.Tools;
using Application.Handlers.Customers;
using Bogus.Extensions;
using FluentAssertions;
using Test.Core.Fixtures;
using Test.Tools.Helpers;
using Xunit;
using static Application.Contracts.Customer.Queries.GetAllCustomers;

namespace Test.Core.Handlers.Customers;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class GetAllCustomersTests : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;
    private readonly GetAllCustomersHandler _handler;

    public GetAllCustomersTests(CoreDatabaseFixture database)
    {
        _database = database;
        _handler = new GetAllCustomersHandler(_database.Context, new PaginationConfiguration(10));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public async Task Handler_Should_ReturnNonEmptyPage(int page)
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);
        var query = new Query(page);

        var response = await _handler.Handle(query, default);

        response.Page.Customers.Should().NotBeNull();
        response.Page.Page.Should().Be(page);
        response.Page.Customers.Count().Should().Be(10);
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