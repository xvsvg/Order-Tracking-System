using System.Net;
using Application.DataAccess.Contracts;
using FastEndpoints;
using FluentAssertions;
using LanguageExt.Pipes;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Couriers;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Courier.Commands.DeleteCourier;

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
        var command = new Command(courier.PersonId);

        var response = await Client
            .DELETEAsync<DeleteCourierEndpoint, Command>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}