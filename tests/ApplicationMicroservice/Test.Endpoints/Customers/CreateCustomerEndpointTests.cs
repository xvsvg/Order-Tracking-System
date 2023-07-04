using System.Net;
using FastEndpoints;
using FluentAssertions;
using Presentation.Endpoints.Customers;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Customer.Commands.CreateCustomer;

namespace Test.Endpoints.Customers;

[Collection(nameof(WebFactoryCollection))]
public class CreateCustomerEndpointTests : IAsyncLifetime
{
    private readonly WebFactory _factory;
    private readonly HttpClient _client;

    public CreateCustomerEndpointTests(WebFactory factory)
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
            .POSTAsync<CreateCustomerEndpoint, Command, Response>(command);
        
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Should().NotBeNull();
        result!.Customer.Should().NotBeNull();
        result!.Customer.Name.Should().Be("John Martin Doe");
        result.Customer.ContactInfo.Should().Contain("whatever@gmail.com");
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
            .POSTAsync<CreateCustomerEndpoint, Command, Response>(command);
        
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeNull();
        result!.Customer.Should().BeNull();
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