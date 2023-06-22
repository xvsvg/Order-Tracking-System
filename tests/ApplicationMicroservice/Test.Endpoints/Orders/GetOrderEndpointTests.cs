using FakeItEasy;
using FastEndpoints;
using FluentAssertions;
using Infrastructure.Mapping.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Presentation.Endpoints.Order;
using Test.Core.Fixtures;
using Test.Core.Helpers;
using static Application.Contracts.Order.Queries.GetOrder;

namespace Test.Endpoints.Orders;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class GetOrderEndpointTests : IAsyncLifetime, IClassFixture<CoreDatabaseFixture>
{
    private readonly CoreDatabaseFixture _database;

    public GetOrderEndpointTests(CoreDatabaseFixture database)
    {
        _database = database;
    }

    [Fact]
    public async Task GetOrderById_Should_NotFind()
    {
        var id = Guid.NewGuid();
        var endpoint = Factory.Create<GetOrderEndpoint>(ctx =>
            {
                ctx.Request.RouteValues.Add("id", id.ToString());
                ctx.Request.QueryString.Add("id", id.ToString());
            },
            A.Fake<IMediator>());

        await endpoint.HandleAsync(new Query(id), default);
        var response = endpoint.Response;

        response.Order.Should().BeNull();
    }

    [Fact]
    public async Task GetOrderById_Should_Find()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);

        var order = await _database.Context.Orders.FirstAsync();
        var mediator = new Mock<IMediator>();

        mediator
            .Setup(x => x.Send(It.IsAny<Query>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response(order.ToDto()));

        var endpoint = Factory.Create<GetOrderEndpoint>(ctx =>
            {
                ctx.Request.RouteValues.Add("id", order.OrderId.ToString());
                ctx.Request.QueryString.Add("id", order.OrderId.ToString());
            },
            mediator.Object);

        await endpoint.HandleAsync(new Query(order.OrderId), default);
        var response = endpoint.Response;

        response.Order.Should().NotBeNull();
        response.Order.Id.Should().Be(order.OrderId);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _database.ResetAsync();
    }
}