using Application.Contracts.Customer.Queries;
using Application.Contracts.Tools;
using Application.Handlers.Customers;
using FluentAssertions;
using Test.Core.ClassData;
using Test.Core.Fixtures;
using Xunit;

namespace Test.Core.Handlers.Customers;

public class GetAllCustomersTests : TestBase
{
    private readonly GetAllCustomersHandler _handler;

    public GetAllCustomersTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new GetAllCustomersHandler(database.Context, new PaginationConfiguration(10));
    }

    [Theory]
    [ClassData(typeof(HandlerTestData))]
    public async Task Handler_Should_ReturnNonEmptyPage(int page)
    {
        var query = new GetAllCustomers.Query(page);

        var response = await _handler.Handle(query, default);

        response.Page.Customers.Should().NotBeNull();
        response.Page.Page.Should().Be(page);
        response.Page.Customers.Count().Should().Be(10);
    }
}