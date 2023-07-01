using System.Net;
using FastEndpoints;
using FluentAssertions;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Order.Queries.GetAllOrders;

namespace Test.Endpoints.Orders;

[Collection(nameof(WebFactoryCollection))]
public class GetAllOrdersEndpointTests : IAsyncLifetime
{
    private readonly WebFactory _factory;
    private readonly HttpClient _client;

    public GetAllOrdersEndpointTests(WebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetOrdersFromPage_ShouldFind()
    {
        var query = new Query(1);

        var (response, result) = await _client
            .GETAsync<Query, Response>($"api/orders?page={query.Page}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.PartialContent);
        result.Should().NotBeNull();
        result!.Page.Page.Should().Be(1);
        result!.Page.Orders.Count().Should().Be(1);
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