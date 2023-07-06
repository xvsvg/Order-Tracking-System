using System.Net;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Order.Queries.GetOrder;

namespace Test.Endpoints.Orders;

public class GetOrderEndpointTests : EndpointTestBase
{
    public GetOrderEndpointTests(WebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetOrderById_Should_NotFind()
    {
        var query = new Query(Guid.NewGuid());

        var (response, result) = await Client
            .GETAsync<Query, Response>($"api/orders/{query.Id}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetOrderById_Should_Find()
    {
        var order = await Database.Orders.FirstAsync();
        var query = new Query(order.OrderId);

        var (response, result) = await Client
            .GETAsync<Query, Response>($"api/orders/{query.Id}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Order.Should().NotBeNull();
        result!.Order!.Id.Should().Be(order.OrderId);
    }
}