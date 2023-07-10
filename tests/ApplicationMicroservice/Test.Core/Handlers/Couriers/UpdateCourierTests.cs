using Application.Contracts.Courier.Commands;
using Application.Handlers.Couriers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;

namespace Test.Core.Handlers.Couriers;

public class UpdateCourierTests : TestBase
{
    private readonly UpdateCourierHandler _handler;

    public UpdateCourierTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new UpdateCourierHandler(database.Context);
    }

    [Fact]
    public async Task UpdateCourier_Should_NotThrow()
    {
        var courier = await Database.Context.Couriers.FirstAsync();
        var command = new UpdateCourier.Command(
            courier.PersonId,
            "John",
            "Martin",
            "Doe",
            courier.ContactInfo.Select(x => x.Contact));
        Database.Context.Entry(courier).State = EntityState.Detached;

        var response = await _handler.Handle(command, default);

        response.Courier.Name.Should().Be("John Martin Doe");
    }
}