using System.Net;
using FastEndpoints;
using FluentAssertions;
using Infrastructure.DataAccess.DatabaseContexts;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Order;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Order.Commands.CreateOrder;

namespace Test.Endpoints.Orders;

[Collection(nameof(WebFactoryCollection))]
public class CreateOrderEndpointTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly DatabaseContext _database;
    private readonly WebFactory _factory;

    public CreateOrderEndpointTests(WebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _database = factory.Context;
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
    public async Task CreateValidOrder_ShouldPassValidation()
    {
        await SeedingHelper.SeedDatabaseAsync(_database);
        var customer = await _database.Customers.FirstAsync();

        var command = new Command(
            "Whatever",
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(2),
            null,
            customer.PersonId);

        var (response, result) = await _client
            .POSTAsync<CreateOrderEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Should().NotBeNull();
        result!.Order.CourierId.Should().Be(null);
        result.Order.CustomerId.Should().Be(customer.PersonId);
        result.Order.Name.Should().Be("Whatever");
    }

    [Fact]
    public async Task CreateInvalidOrder_ShouldNotPassValidation()
    {
        await SeedingHelper.SeedDatabaseAsync(_database);
        var customer = await _database.Customers.FirstAsync();

        var command = new Command(
            "djsklajd123jilkfg[",
            DateTime.UtcNow,
            DateTime.UtcNow,
            null,
            customer.PersonId);

        var (response, result) = await _client
            .POSTAsync<CreateOrderEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeNull();
        result!.Order.Should().BeNull();
    }
}