using System.Net;
using Application.DataAccess.Contracts;
using FastEndpoints;
using FluentAssertions;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Courier.Queries.GetCourier;

namespace Test.Endpoints.Couriers;

[Collection(nameof(WebFactoryCollection))]
public class GetCourierEndpointTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly IDatabaseContext _context;
    private readonly WebFactory _factory;

    public GetCourierEndpointTests(WebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _context = factory.Context;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _factory.ResetAsync();
    }

    [Fact]
    public async Task GetCourier_ShouldFind()
    {
        await SeedingHelper.SeedDatabaseAsync(_context);
        var courier = await _context.Couriers.FirstAsync();
        var query = new Query(courier.PersonId);

        var (response, result) = await _client
            .GETAsync<Query, Response>($"api/couriers/{query.Id}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Courier.Should().NotBeNull();
        result!.Courier!.Id.Should().Be(query.Id);
    }

    [Fact]
    public async Task GetCourier_ShouldNotFind()
    {
        await SeedingHelper.SeedDatabaseAsync(_context);
        var query = new Query(Guid.NewGuid());

        var (response, _) = await _client
            .GETAsync<Query, Response>($"api/couriers/{query.Id}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}