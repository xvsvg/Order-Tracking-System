using System.Net;
using FastEndpoints;
using FluentAssertions;
using Infrastructure.DataAccess.DatabaseContexts;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Order;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Order.Commands.UpdateOrder;

namespace Test.Endpoints.Orders;

[Collection(nameof(WebFactoryCollection))]
public class UpdateOrderEndpointTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly DatabaseContext _database;
    private readonly WebFactory _factory;

    public UpdateOrderEndpointTests(WebFactory factory)
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
    public async Task UpdateOrder_ShouldPassValidation()
    {
        await SeedingHelper.SeedDatabaseAsync(_database);
        var order = await _database.Orders.FirstAsync();

        var command = new Command(
            order.OrderId,
            "New",
            order.DispatchDate,
            order.DeliveryDate,
            order.Courier?.PersonId,
            order.Customer.PersonId);

        var (response, result) = await _client
            .PUTAsync<UpdateOrderEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Order.Name.Should().Be("New");
    }

    [Fact]
    public async Task UpdateOrder_ShouldNot_PassValidation()
    {
        await SeedingHelper.SeedDatabaseAsync(_database);
        var order = await _database.Orders.FirstAsync();

        var command = new Command(
            order.OrderId,
            "sa",
            order.DispatchDate,
            order.DeliveryDate,
            order.Courier?.PersonId,
            order.Customer.PersonId);

        var (response, result) = await _client
            .PUTAsync<UpdateOrderEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}