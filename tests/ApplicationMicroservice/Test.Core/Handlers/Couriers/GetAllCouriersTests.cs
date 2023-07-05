using Application.Contracts.Tools;
using Application.Handlers.Couriers;
using FluentAssertions;
using Test.Core.Fixtures;
using Test.Tools.Helpers;
using Xunit;
using static Application.Contracts.Courier.Queries.GetAllCouriers;

namespace Test.Core.Handlers.Couriers;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class GetAllCouriersTests : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;
    private readonly GetAllCouriersHandler _handler;

    public GetAllCouriersTests(CoreDatabaseFixture database)
    {
        _database = database;
        _handler = new GetAllCouriersHandler(_database.Context, new PaginationConfiguration(10));
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

        response.Page.Page.Should().Be(page);
        response.Page.Couriers.Count().Should().Be(10);
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