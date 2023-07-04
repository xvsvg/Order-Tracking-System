using Application.Contracts.Tools;
using Application.Dto;
using Application.Dto.Pages;
using Application.Handlers.Orders;
using FluentAssertions;
using Test.Core.Fixtures;
using Test.Tools.Helpers;
using Xunit;
using static Application.Contracts.Order.Queries.GetAllOrders;

namespace Test.Core.Handlers.Orders;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class GetAllOrdersTests : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;
    private readonly GetAllOrdersHandler _handler;

    public GetAllOrdersTests(CoreDatabaseFixture database)
    {
        _database = database; 
        _handler = new GetAllOrdersHandler(_database.Context, new PaginationConfiguration(10));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public async Task Handle_Should_ReturnNonEmptyPage(int page)
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);
        var query = new Query(page);

        var response = await _handler.Handle(query, default);

        response.Page.Orders.Should().NotBeEmpty();
        response.Page.Page.Should().Be(page);
        response.Page.Orders.Count().Should().Be(10);
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