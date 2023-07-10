using System.Net;
using Application.Contracts.Order.Queries;
using FastEndpoints;
using FluentAssertions;
using Test.Endpoints.ClassData;
using Test.Endpoints.Fixtures;
using Xunit;

namespace Test.Endpoints.Orders;

public class GetAllOrdersEndpointTests : EndpointTestBase
{
    public GetAllOrdersEndpointTests(WebFactory factory) : base(factory)
    {
    }

    [Theory]
    [ClassData(typeof(EndpointTestData))]
    public async Task GetOrdersFromPage_Should_ReturnNonEmptyPage(int page)
    {
        var query = new GetAllOrders.Query(page);

        var (response, result) = await Client
            .GETAsync<GetAllOrders.Query, GetAllOrders.Response>($"api/orders?page={query.Page}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.PartialContent);
        result.Should().NotBeNull();
        result!.Page.Page.Should().Be(page);
        result!.Page.Orders.Count().Should().Be(1);
    }
}