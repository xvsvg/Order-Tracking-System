using System.Net;
using Application.Contracts.Customer.Commands;
using FastEndpoints;
using FluentAssertions;
using Presentation.Endpoints.Customers;
using Test.Endpoints.Fixtures;
using Xunit;

namespace Test.Endpoints.Customers;

public class CreateCustomerEndpointTests : EndpointTestBase
{
    public CreateCustomerEndpointTests(WebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateValidCustomer_Should_PassValidation()
    {
        var command = new CreateCustomer.Command(
            "John",
            "Martin",
            "Doe",
            new[] { "whatever@gmail.com" });

        var (response, result) = await Client
            .POSTAsync<CreateCustomerEndpoint, CreateCustomer.Command, CreateCustomer.Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Should().NotBeNull();
        result!.Customer.Should().NotBeNull();
        result!.Customer.Name.Should().Be("John Martin Doe");
        result.Customer.ContactInfo.Should().Contain("whatever@gmail.com");
    }

    [Fact]
    public async Task CreateInvalidCustomer_Should_NotPassValidation()
    {
        var command = new CreateCustomer.Command(
            "john",
            "Mart1n",
            "do",
            new[] { "whatever@gmail.com" });

        var (response, result) = await Client
            .POSTAsync<CreateCustomerEndpoint, CreateCustomer.Command, CreateCustomer.Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeNull();
        result!.Customer.Should().BeNull();
    }
}