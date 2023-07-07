using Infrastructure.Mapping.Orders;
using Infrastructure.Mapping.People;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;

namespace Test.Core.Mapping;

public class MappingTest : TestBase
{
    public MappingTest(CoreDatabaseFixture database) : base(database)
    {
    }

    [Fact]
    public async Task MapEntities_Should_NotHaveCircuits()
    {
        var order = await Database.Context.Orders.FirstAsync();

        var orderDto = order.ToDto();
        var customerDto = order.Customer.ToDto();
        var courierDto = order.Courier?.ToDto();
    }
}