using System.Net;
using Application.Contracts.Courier.Queries;
using FastEndpoints;
using FluentAssertions;
using Test.Endpoints.ClassData;
using Test.Endpoints.Fixtures;
using Xunit;

namespace Test.Endpoints.Couriers;

public class GetAllCouriersEndpointTests : EndpointTestBase
{
    public GetAllCouriersEndpointTests(WebFactory factory) : base(factory)
    {
    }

    [Theory]
    [ClassData(typeof(EndpointTestData))]
    public async Task GetAllCouriersFromPage_Should_ReturnNonEmptyPage(int page)
    {
        var query = new GetAllCouriers.Query(page);

        var (response, result) = await Client
            .GETAsync<GetAllCouriers.Query, GetAllCouriers.Response>($"api/couriers?page={query.Page}", query);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.PartialContent);
        result.Should().NotBeNull();
        result!.Page.Page.Should().Be(page);
        result!.Page.Couriers.Count().Should().Be(1);
    }
}