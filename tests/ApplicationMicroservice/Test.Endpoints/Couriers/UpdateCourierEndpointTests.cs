using System.Net;
using Application.DataAccess.Contracts;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Couriers;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Courier.Commands.UpdateCourier;

namespace Test.Endpoints.Couriers;

[Collection(nameof(WebFactoryCollection))]
public class UpdateCourierEndpointTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly IDatabaseContext _context;
    private readonly WebFactory _factory;

    public UpdateCourierEndpointTests(WebFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
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
    public async Task UpdateCustomer_ShouldPassValidation()
    {
        var courier = await _context.Couriers.FirstAsync();

        var command = new Command(
            courier.PersonId,
            "John",
            "Martin",
            "Doe",
            courier.ContactInfo.Select(x => x.Contact));

        var (response, result) = await _client
            .PUTAsync<UpdateCourierEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateCustomer_ShouldNotPassValidation()
    {
        var courier = await _context.Couriers.FirstAsync();

        var command = new Command(
            courier.PersonId,
            "john",
            "Mart1n",
            "Do",
            courier.ContactInfo.Select(x => x.Contact));

        var (response, result) = await _client
            .PUTAsync<UpdateCourierEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}