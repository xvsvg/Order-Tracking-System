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

    public GetAllOrdersTests(CoreDatabaseFixture database)
    {
        _database = database;
    }

    [Fact]
    public async Task Handle_Should_ReturnNonEmptyPage()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);
        var query = new Query(1);
        var handler = new GetAllOrdersHandler(_database.Context, new PaginationConfiguration(10));

        var response = await handler.Handle(query, default);

        var result = response.Match(
            x => x.Page,
            x => new OrderPageDto(Array.Empty<OrderDto>(), 0, 0));

        result.Orders.Should().NotBeEmpty();
        result.Page.Should().Be(1);
        result.Orders.Count().Should().Be(10);
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