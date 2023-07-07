using Application.Contracts.Tools;
using Application.Handlers.Couriers;
using FluentAssertions;
using Infrastructure.Seeding.Helpers;
using Test.Core.ClassData;
using Test.Core.Fixtures;
using Xunit;
using static Application.Contracts.Courier.Queries.GetAllCouriers;

namespace Test.Core.Handlers.Couriers;

public class GetAllCouriersTests : TestBase
{
    private readonly GetAllCouriersHandler _handler;

    public GetAllCouriersTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new GetAllCouriersHandler(database.Context, new PaginationConfiguration(10));
    }

    [Theory]
    [ClassData(typeof(HandlerTestData))]
    public async Task Handle_Should_ReturnNonEmptyPage(int page)
    {
        var query = new Query(page);

        var response = await _handler.Handle(query, default);

        response.Page.Page.Should().Be(page);
        response.Page.Couriers.Count().Should().Be(10);
    }
}