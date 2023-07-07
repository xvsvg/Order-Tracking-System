using System.Net;
using Application.DataAccess.Contracts;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Customer.Commands.RemoveOrder;

namespace Test.Endpoints.Customers;

public class RemoveOrderEndpointTests : EndpointTestBase
{
    public RemoveOrderEndpointTests(WebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task RemoveOrder_ShouldNotThrow()
    {
        var customer = await Database.Customers.FirstAsync(x => x.OrderHistory.Any());
        var order = customer!.OrderHistory.First();
        var command = new Command(customer.PersonId, order.OrderId);

        var response = await Client
            .DELETEAsync<Command, EmptyResponse>($"api/customers/{customer.PersonId}/orders/{order.OrderId}", command);

        response.Should().NotBeNull();
        response!.Response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task RemoveOrder_ShouldThrow()
    {
        var command = new Command(Guid.NewGuid(), Guid.NewGuid());

        var response = await Client
            .DELETEAsync<Command, EmptyResponse>($"api/customers/{command.CustomerId}/orders/{command.OrderId}",
                command);

        response.Should().NotBeNull();
        response!.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}