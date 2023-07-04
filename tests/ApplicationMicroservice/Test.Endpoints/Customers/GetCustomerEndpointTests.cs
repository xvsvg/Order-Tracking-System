using System.Net;
using Application.DataAccess.Contracts;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Endpoints.Fixtures;
using Test.Tools.Helpers;
using Xunit;
using static Application.Contracts.Customer.Queries.GetCustomer;

namespace Test.Endpoints.Customers;

[Collection(nameof(WebFactoryCollection))]
public class GetCustomerEndpointTests : IAsyncLifetime
{
    private readonly WebFactory _factory;
    private readonly HttpClient _client;
    private readonly IDatabaseContext _context;

    public GetCustomerEndpointTests(WebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _context = factory.Context;
    }

    [Fact]
    public async Task GetCustomerById_Should_Find()
    {
        await SeedingHelper.SeedDatabaseAsync(_context);
        var customer = await _context.Customers.FirstAsync();
        var query = new Query(customer.PersonId);

        var (response, result) = await _client
            .GETAsync<Query, Response>($"api/customers/{query.Id}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Customer.Should().NotBeNull();
        result!.Customer!.Id.Should().Be(customer.PersonId);
    }

    [Fact]
    public async Task GetCustomerById_Should_NotFind()
    {
        await SeedingHelper.SeedDatabaseAsync(_context);
        var customer = await _context.Customers.FirstAsync();
        var query = new Query(Guid.NewGuid());

        var (response, result) = await _client
            .GETAsync<Query, Response>($"api/customers/{query.Id}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Should().BeNull();

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