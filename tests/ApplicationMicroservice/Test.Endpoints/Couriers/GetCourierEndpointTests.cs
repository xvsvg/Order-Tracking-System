using System.Net;
using Application.Contracts.Courier.Queries;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Endpoints.Fixtures;
using Xunit;

namespace Test.Endpoints.Couriers;

public class GetCourierEndpointTests : EndpointTestBase
{
    public GetCourierEndpointTests(WebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetCourierById_Should_Find()
    {
        var courier = await Database.Couriers.FirstAsync();
        var query = new GetCourier.Query(courier.PersonId);

        var (response, result) = await Client
            .GETAsync<GetCourier.Query, GetCourier.Response>($"api/couriers/{query.Id}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Courier.Should().NotBeNull();
        result!.Courier!.Id.Should().Be(query.Id);
    }

    [Fact]
    public async Task GetCourierById_Should_NotFind()
    {
        var query = new GetCourier.Query(Guid.NewGuid());

        var (response, _) = await Client
            .GETAsync<GetCourier.Query, GetCourier.Response>($"api/couriers/{query.Id}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}