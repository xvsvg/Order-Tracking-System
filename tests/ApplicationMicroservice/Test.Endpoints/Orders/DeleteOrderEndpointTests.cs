using System.Net;
using Application.DataAccess.Contracts;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Order;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Order.Commands.DeleteOrder;

namespace Test.Endpoints.Orders;

[Collection(nameof(WebFactoryCollection))]
public class DeleteOrderEndpointTests : IAsyncLifetime
{
    private readonly WebFactory _factory;
    private readonly HttpClient _client;
    private readonly IDatabaseContext _context;

    public DeleteOrderEndpointTests(WebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _context = factory.Context;
    }

    [Fact]
    public async Task DeleteOrder_ShouldNotThrow()
    {
        var order = await _context.Orders.FirstAsync();

        var command = new Command(order.OrderId);

        var response = await _client
            .DELETEAsync<DeleteOrderEndpoint, Command>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
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