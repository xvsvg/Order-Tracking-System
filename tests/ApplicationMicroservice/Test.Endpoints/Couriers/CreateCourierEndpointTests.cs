using System.Net;
using FastEndpoints;
using FluentAssertions;
using Presentation.Endpoints.Couriers;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Courier.Commands.CreateCourier;

namespace Test.Endpoints.Couriers;

public class CreateCourierEndpointTests : EndpointTestBase
{
    public CreateCourierEndpointTests(WebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateValidCustomer_Should_PassValidation()
    {
        var command = new Command(
            "John",
            "Martin",
            "Doe",
            new[] { "whatever@gmail.com" });

        var (response, result) = await Client
            .POSTAsync<CreateCourierEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Should().NotBeNull();
        result!.Courier.Should().NotBeNull();
        result!.Courier.Name.Should().Be("John Martin Doe");
        result.Courier.ContactInfo.Should().Contain("whatever@gmail.com");
    }

    [Fact]
    public async Task CreateInvalidCustomer_Should_NotPassValidation()
    {
        var command = new Command(
            "john",
            "Mart1n",
            "do",
            new[] { "whatever@gmail.com" });

        var (response, result) = await Client
            .POSTAsync<CreateCourierEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeNull();
        result!.Courier.Should().BeNull();
    }
}