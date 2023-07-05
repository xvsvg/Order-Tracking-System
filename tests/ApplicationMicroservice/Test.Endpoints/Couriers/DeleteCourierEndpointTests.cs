using System.Net;
using Application.DataAccess.Contracts;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Couriers;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Courier.Commands.DeleteCourier;

namespace Test.Endpoints.Couriers;

[Collection(nameof(WebFactoryCollection))]
public class DeleteCourierEndpointTests : IAsyncLifetime
{
    private readonly WebFactory _factory;
    private readonly HttpClient _client;
    private readonly IDatabaseContext _context;

    public DeleteCourierEndpointTests(WebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _context = factory.Context;
    }

    [Fact]
    public async Task DeleteCustomer_ShouldNotThrow()
    {
        var courier = await _context.Couriers.FirstAsync();

        var command = new Command(courier.PersonId);

        var response = await _client
            .DELETEAsync<DeleteCourierEndpoint, Command>(command);

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