using System.Net;
using FastEndpoints;
using FluentAssertions;
using Presentation.Endpoints.Couriers;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Courier.Commands.CreateCourier;

namespace Test.Endpoints.Couriers;

[Collection(nameof(WebFactoryCollection))]
public class CreateCourierEndpointTests : IAsyncLifetime
{
    private readonly WebFactory _factory;
    private readonly HttpClient _client;

    public CreateCourierEndpointTests(WebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateValidCustomer_ShouldPassValidation()
    {
        var command = new Command(
            "John",
            "Martin",
            "Doe",
            new[] { "whatever@gmail.com" });

        var (response, result) = await _client
            .POSTAsync<CreateCourierEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Should().NotBeNull();
        result!.Courier.Should().NotBeNull();
        result!.Courier.Name.Should().Be("John Martin Doe");
        result.Courier.ContactInfo.Should().Contain("whatever@gmail.com");
    }

    [Fact]
    public async Task CreateInvalidCustomer_ShouldNotPassValidation()
    {
        var command = new Command(
            "john",
            "Mart1n",
            "do",
            new[] { "whatever@gmail.com" });

        var (response, result) = await _client
            .POSTAsync<CreateCourierEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeNull();
        result!.Courier.Should().BeNull();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _factory.ResetAsync();
    }
}