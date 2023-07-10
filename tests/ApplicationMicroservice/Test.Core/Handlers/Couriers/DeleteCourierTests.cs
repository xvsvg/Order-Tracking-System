using Application.Contracts.Courier.Commands;
using Application.Handlers.Couriers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;

namespace Test.Core.Handlers.Couriers;

public class DeleteCourierTests : TestBase
{
    private readonly DeleteCourierHandler _handler;

    public DeleteCourierTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new DeleteCourierHandler(database.Context);
    }


    [Fact]
    public async Task DeleteCourier_Should_SuccessfullyDelete()
    {
        var courier = await Database.Context.Couriers.FirstAsync();
        var command = new DeleteCourier.Command(courier.PersonId);

        var response = await _handler.Handle(command, default);

        var result = response.Match(
            x => courier,
            err => null!);

        result.Should().NotBeNull();
    }
}