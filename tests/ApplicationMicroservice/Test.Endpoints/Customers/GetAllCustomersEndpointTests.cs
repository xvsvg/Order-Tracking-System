using System.Net;
using Application.Contracts.Customer.Queries;
using FastEndpoints;
using FluentAssertions;
using Test.Endpoints.ClassData;
using Test.Endpoints.Fixtures;
using Xunit;

namespace Test.Endpoints.Customers;

public class GetAllCustomersEndpointTests : EndpointTestBase
{
    public GetAllCustomersEndpointTests(WebFactory factory) : base(factory)
    {
    }

    [Theory]
    [ClassData(typeof(EndpointTestData))]
    public async Task GetCustomersFromPage_Should_ReturnNonEmptyPage(int page)
    {
        var query = new GetAllCustomers.Query(page);

        var (response, result) = await Client
            .GETAsync<GetAllCustomers.Query, GetAllCustomers.Response>($"api/customers?page={query.Page}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.PartialContent);
        result.Should().NotBeNull();
        result!.Page.Page.Should().Be(page);
        result!.Page.Customers.Count().Should().Be(1);
    }
}