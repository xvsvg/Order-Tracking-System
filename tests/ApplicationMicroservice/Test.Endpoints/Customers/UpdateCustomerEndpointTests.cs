using System.Net;
using Application.DataAccess.Contracts;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Customers;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Customer.Commands.UpdateCustomer;

namespace Test.Endpoints.Customers;

[Collection(nameof(WebFactoryCollection))]
public class UpdateCustomerEndpointTests : IAsyncLifetime
{
    private readonly WebFactory _factory;
    private readonly HttpClient _client;
    private readonly IDatabaseContext _context;

    public UpdateCustomerEndpointTests(WebFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _context = factory.Context;
    }

    [Fact]
    public async Task UpdateCustomer_ShouldPassValidation()
    {
        var customer = await _context.Customers.FirstAsync();

        var command = new Command(
            customer.PersonId,
            "John",
            "Martin",
            "Doe",
            customer.ContactInfo.Select(x => x.Contact));

        var (response, result) = await _client
            .PUTAsync<UpdateCustomerEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task UpdateCustomer_ShouldNotPassValidation()
    {
        var customer = await _context.Customers.FirstAsync();

        var command = new Command(
            customer.PersonId,
            "john",
            "Mart1n",
            "Do",
            customer.ContactInfo.Select(x => x.Contact));

        var (response, result) = await _client
            .PUTAsync<UpdateCustomerEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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