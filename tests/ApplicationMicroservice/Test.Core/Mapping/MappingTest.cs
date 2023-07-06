using Infrastructure.Mapping.Orders;
using Infrastructure.Mapping.People;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;

namespace Test.Core.Mapping;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class MappingTest : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;

    public MappingTest(CoreDatabaseFixture database)
    {
        _database = database;
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
    public async Task MapEntities_Should_NotDrop()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);

        var order = await _database.Context.Orders.FirstAsync();

        var orderDto = order.ToDto();
        var customerDto = order.Customer.ToDto();
        var courierDto = order.Courier?.ToDto();
    }
}