using System.Net;
using Application.DataAccess.Contracts;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Customers;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Customer.Commands.DeleteCustomer;

namespace Test.Endpoints.Customers;

public class DeleteCustomerEndpointTests : EndpointTestBase
{
    public DeleteCustomerEndpointTests(WebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task DeleteCustomer_Should_NotThrow()
    {
        var customer = await Database.Customers.FirstAsync();

        var command = new Command(customer.PersonId);

        var response = await Client
            .DELETEAsync<DeleteCustomerEndpoint, Command>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}