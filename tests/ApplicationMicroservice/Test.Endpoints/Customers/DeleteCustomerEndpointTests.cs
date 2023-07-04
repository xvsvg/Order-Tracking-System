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

[Collection(nameof(WebFactoryCollection))]
public class DeleteCustomerEndpointTests : IAsyncLifetime
{
    private readonly WebFactory _factory;
    private readonly HttpClient _client;
    private readonly IDatabaseContext _context;

    public DeleteCustomerEndpointTests(WebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _context = factory.Context;
    }

    [Fact]
    public async Task DeleteCustomer_ShouldNotThrow()
    {
        var customer = await _context.Customers.FirstAsync();
        
        var command = new Command(customer.PersonId);

        var response = await _client
            .DELETEAsync<DeleteCustomerEndpoint, Command>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
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