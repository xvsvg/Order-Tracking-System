using System.Net;
using Application.DataAccess.Contracts;
using Domain.Core.Implementations;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Couriers;
using Test.Endpoints.Fixtures;
using Xunit;
using static Application.Contracts.Courier.Commands.UpdateCourier;

namespace Test.Endpoints.Couriers;

public class UpdateCourierEndpointTests : EndpointTestBase
{
    private readonly Courier _courier;
    public UpdateCourierEndpointTests(WebFactory factory) : base(factory)
    {
        var proxy = new Lazy<Task<Courier>>(async () => await Database.Couriers.FirstAsync());

        _courier = proxy.Value.Result;
    }

    [Fact]
    public async Task UpdateCustomer_Should_PassValidation()
    {
        var command = new Command(
            _courier.PersonId,
            "John",
            "Martin",
            "Doe",
            _courier.ContactInfo.Select(x => x.Contact));

        var (response, result) = await Client
            .PUTAsync<UpdateCourierEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateCustomer_Should_NotPassValidation()
    {
        var command = new Command(
            _courier.PersonId,
            "john",
            "Mart1n",
            "Do",
            _courier.ContactInfo.Select(x => x.Contact));

        var (response, result) = await Client
            .PUTAsync<UpdateCourierEndpoint, Command, Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}