using Application.Contracts.Customer.Queries;
using Application.Handlers.Customers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;

namespace Test.Core.Handlers.Customers;

public class GetCustomerTests : TestBase
{
    private readonly GetCustomerHandler _handler;

    public GetCustomerTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new GetCustomerHandler(Database.Context);
    }

    [Fact]
    public async Task GetCustomer_Should_NotFind()
    {
        var query = new GetCustomer.Query(Guid.NewGuid());

        var response = await _handler.Handle(query, default);

        response.Should().NotBeNull();
        response!.Customer.Should().BeNull();
    }

    [Fact]
    public async Task GetCustomer_Should_Find()
    {
        var customer = await Database.Context.Customers.FirstAsync();
        var query = new GetCustomer.Query(customer.PersonId);

        var response = await _handler.Handle(query, default);

        response.Should().NotBeNull();
        response!.Customer.Should().NotBeNull();
        response!.Customer!.Id.Should().Be(customer.PersonId);
    }
}