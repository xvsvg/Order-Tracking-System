using System.Net;
using Application.Contracts.Order.Commands;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Order;
using Test.Endpoints.Fixtures;
using Xunit;
using Order = Domain.Core.Implementations.Order;

namespace Test.Endpoints.Orders;

public class UpdateOrderEndpointTests : EndpointTestBase
{
    private readonly Order _order;

    public UpdateOrderEndpointTests(WebFactory factory) : base(factory)
    {
        var proxy = new Lazy<Task<Order>>(async () =>
            await Database.Orders.FirstAsync());

        _order = proxy.Value.Result;
    }

    [Fact]
    public async Task UpdateOrder_Should_PassValidation()
    {
        var command = new UpdateOrder.Command(
            _order.OrderId,
            "New",
            _order.DispatchDate,
            _order.DeliveryDate,
            _order.Courier?.PersonId,
            _order.Customer.PersonId);

        var (response, result) = await Client
            .PUTAsync<UpdateOrderEndpoint, UpdateOrder.Command, UpdateOrder.Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Order.Name.Should().Be("New");
    }

    [Fact]
    public async Task UpdateOrder_Should_NotPassValidation()
    {
        var command = new UpdateOrder.Command(
            _order.OrderId,
            "sa",
            _order.DispatchDate,
            _order.DeliveryDate,
            _order.Courier?.PersonId,
            _order.Customer.PersonId);

        var (response, _) = await Client
            .PUTAsync<UpdateOrderEndpoint, UpdateOrder.Command, UpdateOrder.Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}