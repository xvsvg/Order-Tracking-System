using System.Net;
using Application.Contracts.Order.Queries;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Endpoints.Fixtures;
using Xunit;

namespace Test.Endpoints.Orders;

public class GetOrderEndpointTests : EndpointTestBase
{
    public GetOrderEndpointTests(WebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetOrderById_Should_NotFind()
    {
        var query = new GetOrder.Query(Guid.NewGuid());

        var (response, result) = await Client
            .GETAsync<GetOrder.Query, GetOrder.Response>($"api/orders/{query.Id}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetOrderById_Should_Find()
    {
        var order = await Database.Orders.FirstAsync();
        var query = new GetOrder.Query(order.OrderId);

        var (response, result) = await Client
            .GETAsync<GetOrder.Query, GetOrder.Response>($"api/orders/{query.Id}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Order.Should().NotBeNull();
        result!.Order!.Id.Should().Be(order.OrderId);
    }
}