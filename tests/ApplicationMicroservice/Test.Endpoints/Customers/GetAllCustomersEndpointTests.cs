using System.Net;
using FastEndpoints;
using FluentAssertions;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Customer.Queries.GetAllCustomers;

namespace Test.Endpoints.Customers;

[Collection(nameof(WebFactoryCollection))]
public class GetAllCustomersEndpointTests : IAsyncLifetime
{
    private readonly WebFactory _factory;
    private readonly HttpClient _client;

    public GetAllCustomersEndpointTests(WebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public async Task GetCustomersFromPage_ShouldFind(int page)
    {
        var query = new Query(page);

        var (response, result) = await _client
            .GETAsync<Query, Response>($"api/customers?page={query.Page}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.PartialContent);
        result.Should().NotBeNull();
        result!.Page.Page.Should().Be(page);
        result!.Page.Customers.Count().Should().Be(1);
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