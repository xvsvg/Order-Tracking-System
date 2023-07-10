using System.Net;
using Application.Contracts.Customer.Queries;
using Domain.Core.Implementations;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Endpoints.Fixtures;
using Xunit;

namespace Test.Endpoints.Customers;

public class GetCustomerEndpointTests : EndpointTestBase
{
    private readonly Customer _customer;

    public GetCustomerEndpointTests(WebFactory factory) : base(factory)
    {
        var proxy = new Lazy<Task<Customer>>(async () =>
            await Database.Customers.FirstAsync());

        _customer = proxy.Value.Result;
    }

    [Fact]
    public async Task GetCustomerById_Should_Find()
    {
        var query = new GetCustomer.Query(_customer.PersonId);

        var (response, result) = await Client
            .GETAsync<GetCustomer.Query, GetCustomer.Response>($"api/customers/{query.Id}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Customer.Should().NotBeNull();
        result!.Customer!.Id.Should().Be(_customer.PersonId);
    }

    [Fact]
    public async Task GetCustomerById_Should_NotFind()
    {
        var query = new GetCustomer.Query(Guid.NewGuid());

        var (response, result) = await Client
            .GETAsync<GetCustomer.Query, GetCustomer.Response>($"api/customers/{query.Id}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.Should().BeNull();
    }
}