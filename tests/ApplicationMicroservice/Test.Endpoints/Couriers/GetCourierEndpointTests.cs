using System.Net;
using Application.DataAccess.Contracts;
using FastEndpoints;
using FluentAssertions;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Courier.Queries.GetCourier;

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
        var query = new Query(courier.PersonId);

        var (response, result) = await Client
            .GETAsync<Query, Response>($"api/couriers/{query.Id}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Courier.Should().NotBeNull();
        result!.Courier!.Id.Should().Be(query.Id);
    }

    [Fact]
    public async Task GetCourierById_Should_NotFind()
    {
        var query = new Query(Guid.NewGuid());

        var (response, _) = await Client
            .GETAsync<Query, Response>($"api/couriers/{query.Id}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}