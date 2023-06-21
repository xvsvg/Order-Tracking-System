using Infrastructure.Mapping.Orders;
using Infrastructure.Mapping.People;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Test.Core.Helpers;
using Xunit;

namespace Test.DataAccess;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class MappingTest : IAsyncLifetime, IClassFixture<CoreDatabaseFixture>
{
    private readonly CoreDatabaseFixture _database;
    
    public MappingTest(CoreDatabaseFixture database)
    {
        _database = database;
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


    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _database.DisposeAsync();
    }
}