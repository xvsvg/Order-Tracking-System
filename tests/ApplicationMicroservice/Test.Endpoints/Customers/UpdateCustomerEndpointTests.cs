using System.Net;
using Application.DataAccess.Contracts;
using Domain.Core.Implementations;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Customers;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Customer.Commands.UpdateCustomer;

namespace Test.Endpoints.Customers;

public class UpdateCustomerEndpointTests : EndpointTestBase
{
    private readonly Customer _customer;

    public UpdateCustomerEndpointTests(WebFactory factory) : base(factory)
    {
        var proxy = new Lazy<Task<Customer>>(async () => await Database.Customers.FirstAsync());

        _customer = proxy.Value.Result;
    }

    [Fact]
    public async Task UpdateCustomer_Should_PassValidation()
    {
        var command = new Command(
            _customer.PersonId,
            "John",
            "Martin",
            "Doe",
            _customer.ContactInfo.Select(x => x.Contact));

        var (response, result) = await Client
            .PUTAsync<UpdateCustomerEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateCustomer_Should_NotPassValidation()
    {
        var command = new Command(
            _customer.PersonId,
            "john",
            "Mart1n",
            "Do",
            _customer.ContactInfo.Select(x => x.Contact));

        var (response, result) = await Client
            .PUTAsync<UpdateCustomerEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}