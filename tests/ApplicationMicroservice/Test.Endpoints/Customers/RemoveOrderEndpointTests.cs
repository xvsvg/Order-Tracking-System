using System.Net;
using Application.Contracts.Customer.Commands;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Endpoints.Fixtures;
using Xunit;

namespace Test.Endpoints.Customers;

public class RemoveOrderEndpointTests : EndpointTestBase
{
    public RemoveOrderEndpointTests(WebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task RemoveOrder_Should_RemoveSuccessfully()
    {
        var customer = await Database.Customers.FirstAsync(x => x.OrderHistory.Any());
        var order = customer!.OrderHistory.First();
        var command = new RemoveOrder.Command(customer.PersonId, order.OrderId);

        var response = await Client
            .DELETEAsync<RemoveOrder.Command, EmptyResponse>(
                $"api/customers/{command.CustomerId}/orders/{command.OrderId}", command);

        response.Should().NotBeNull();
        response!.Response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task RemoveOrder_Should_Throw()
    {
        var command = new RemoveOrder.Command(Guid.NewGuid(), Guid.NewGuid());

        var response = await Client
            .DELETEAsync<RemoveOrder.Command, EmptyResponse>(
                $"api/customers/{command.CustomerId}/orders/{command.OrderId}",
                command);

        response.Should().NotBeNull();
        response!.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}