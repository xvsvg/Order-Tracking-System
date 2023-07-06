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
using Order = Domain.Core.Implementations.Order;

namespace Test.Endpoints.Orders;

public class UpdateOrderEndpointTests : EndpointTestBase
{
    private readonly Lazy<Task<Order>> _orderProxy;
    private readonly Order _order;

    public UpdateOrderEndpointTests(WebFactory factory) : base(factory)
    {
        _orderProxy = new Lazy<Task<Order>>(async () =>
            await Database.Orders.FirstAsync());

        _order = _orderProxy.Value.Result;
    }

    [Fact]
    public async Task UpdateOrder_Should_PassValidation()
    {
        var command = new Command(
            _order.OrderId,
            "New",
            _order.DispatchDate,
            _order.DeliveryDate,
            _order.Courier?.PersonId,
            _order.Customer.PersonId);

        var (response, result) = await Client
            .PUTAsync<UpdateOrderEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Order.Name.Should().Be("New");
    }

    [Fact]
    public async Task UpdateOrder_Should_NotPassValidation()
    {
        var command = new Command(
            _order.OrderId,
            "sa",
            _order.DispatchDate,
            _order.DeliveryDate,
            _order.Courier?.PersonId,
            _order.Customer.PersonId);

        var (response, _) = await Client
            .PUTAsync<UpdateOrderEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}