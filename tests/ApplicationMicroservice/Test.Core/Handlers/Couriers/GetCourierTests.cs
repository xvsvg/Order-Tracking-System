using Application.Contracts.Courier.Queries;
using Application.Handlers.Couriers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;

namespace Test.Core.Handlers.Couriers;

public class GetCourierTests : TestBase
{
    private readonly GetCourierHandler _handler;

    public GetCourierTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new GetCourierHandler(database.Context);
    }

    [Fact]
    public async Task GetCourier_Should_Find()
    {
        var customer = await Database.Context.Couriers.FirstAsync();
        var query = new GetCourier.Query(customer.PersonId);

        var response = await _handler.Handle(query, default);

        response.Courier.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCourier_Should_NotFind()
    {
        var query = new GetCourier.Query(Guid.NewGuid());

        var response = await _handler.Handle(query, default);

        response.Courier.Should().BeNull();
    }
}