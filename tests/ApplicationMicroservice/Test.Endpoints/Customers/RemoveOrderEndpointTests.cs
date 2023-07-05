using System.Net;
using Application.DataAccess.Contracts;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Customers;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Customer.Commands.RemoveOrder;

namespace Test.Endpoints.Customers;

[Collection(nameof(WebFactoryCollection))]
public class RemoveOrderEndpointTests : IAsyncLifetime
{
    private readonly WebFactory _factory;
    private readonly HttpClient _client;
    private readonly IDatabaseContext _context;

    public RemoveOrderEndpointTests(WebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _context = factory.Context;
    }

    [Fact]
    public async Task RemoveOrder_ShouldNotThrow()
    {
        var customer = await _context.Customers
            .FirstAsync(x => x.OrderHistory.Any());

        var order = customer!.OrderHistory.First();

        var command = new Command(customer.PersonId, order.OrderId);

        var response = await _client
            .DELETEAsync<Command, EmptyResponse>($"api/customers/{customer.PersonId}/orders/{order.OrderId}", command);

        response.Should().NotBeNull();
        response!.Response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task RemoveOrder_ShouldThrow()
    {
        var command = new Command(Guid.NewGuid(), Guid.NewGuid());

        var response = await _client
            .DELETEAsync<Command, EmptyResponse>($"api/customers/{command.CustomerId}/orders/{command.OrderId}", command);

        response.Should().NotBeNull();
        response!.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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