using System.Net;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Order.Queries.GetOrder;

namespace Test.Endpoints.Orders;

[Collection(nameof(WebFactoryCollection))]
public class GetOrderEndpointTests : IAsyncLifetime
{
    private readonly WebFactory _factory;
    private readonly HttpClient _client;

    public GetOrderEndpointTests(WebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetOrderById_Should_NotFind()
    {
        var query = new Query(Guid.NewGuid());

        var (response, result) = await _client
            .GETAsync<Query, Response>($"api/orders/{query.Id}",query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetOrderById_Should_Find()
    {
        var order = await _factory.Context.Orders.FirstAsync();
        var query = new Query(order.OrderId);
        
        var (response, result) = await _client
            .GETAsync<Query, Response>($"api/orders/{query.Id}",query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Order.Should().NotBeNull();
        result!.Order!.Id.Should().Be(order.OrderId);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _factory.ResetAsync();
    }
}