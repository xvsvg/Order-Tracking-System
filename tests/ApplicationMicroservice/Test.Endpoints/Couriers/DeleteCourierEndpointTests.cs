using System.Net;
using Application.Contracts.Courier.Commands;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Couriers;
using Test.Endpoints.Fixtures;
using Xunit;

namespace Test.Endpoints.Couriers;

public class DeleteCourierEndpointTests : EndpointTestBase
{
    public DeleteCourierEndpointTests(WebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task DeleteCustomer_Should_DeleteSuccessfully()
    {
        var courier = await Database.Couriers.FirstAsync();
        var command = new DeleteCourier.Command(courier.PersonId);

        var response = await Client
            .DELETEAsync<DeleteCourierEndpoint, DeleteCourier.Command>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}