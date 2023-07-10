using System.Net;
using Application.Contracts.Customer.Commands;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Customers;
using Test.Endpoints.Fixtures;
using Xunit;

namespace Test.Endpoints.Customers;

public class DeleteCustomerEndpointTests : EndpointTestBase
{
    public DeleteCustomerEndpointTests(WebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task DeleteCustomer_Should_DeleteSuccessfully()
    {
        var customer = await Database.Customers.FirstAsync();

        var command = new DeleteCustomer.Command(customer.PersonId);

        var response = await Client
            .DELETEAsync<DeleteCustomerEndpoint, DeleteCustomer.Command>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}