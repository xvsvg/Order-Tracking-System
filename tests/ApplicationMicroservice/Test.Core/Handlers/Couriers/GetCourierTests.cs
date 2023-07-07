using Application.Handlers.Customers;
using FluentAssertions;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;
using static Application.Contracts.Customer.Queries.GetCustomer;

namespace Test.Core.Handlers.Couriers;

public class GetCourierTests : TestBase
{
    private readonly GetCustomerHandler _handler;

    public GetCourierTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new GetCustomerHandler(database.Context);
    }

    [Fact]
    public async Task GetCustomer_Should_Find()
    {
        var customer = await Database.Context.Customers.FirstAsync();
        var query = new Query(customer.PersonId);

        var response = await _handler.Handle(query, default);

        response.Customer.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCustomer_Should_NotFind()
    {
        var query = new Query(Guid.NewGuid());

        var response = await _handler.Handle(query, default);

        response.Customer.Should().BeNull();
    }
}