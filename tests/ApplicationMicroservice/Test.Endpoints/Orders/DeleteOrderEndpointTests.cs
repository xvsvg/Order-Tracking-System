using System.Net;
using Application.Contracts.Order.Commands;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Order;
using Test.Endpoints.Fixtures;
using Xunit;

namespace Test.Endpoints.Orders;

public class DeleteOrderEndpointTests : EndpointTestBase
{
    public DeleteOrderEndpointTests(WebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task DeleteOrder_Should_DeleteSuccessfully()
    {
        var order = await Database.Orders.FirstAsync();
        var command = new DeleteOrder.Command(order.OrderId);

        var response = await Client
            .DELETEAsync<DeleteOrderEndpoint, DeleteOrder.Command>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}