using Application.Contracts.Tools;
using Application.Handlers.Orders;
using FluentAssertions;
using Test.Core.ClassData;
using Test.Core.Fixtures;
using Xunit;
using static Application.Contracts.Order.Queries.GetAllOrders;

namespace Test.Core.Handlers.Orders;

public class GetAllOrdersTests : TestBase
{
    private readonly GetAllOrdersHandler _handler;

    public GetAllOrdersTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new GetAllOrdersHandler(database.Context, new PaginationConfiguration(10));
    }

    [Theory]
    [ClassData(typeof(HandlerTestData))]
    public async Task Handle_Should_ReturnNonEmptyPage(int page)
    {
        var query = new Query(page);

        var response = await _handler.Handle(query, default);

        response.Page.Orders.Should().NotBeEmpty();
        response.Page.Page.Should().Be(page);
        response.Page.Orders.Count().Should().Be(10);
    }
}