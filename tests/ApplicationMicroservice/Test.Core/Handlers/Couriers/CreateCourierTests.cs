using Application.Handlers.Couriers;
using FluentAssertions;
using Infrastructure.Seeding.Helpers;
using Test.Core.Fixtures;
using Xunit;
using static Application.Contracts.Courier.Commands.CreateCourier;

namespace Test.Core.Handlers.Couriers;

public class CreateCourierTests : TestBase
{
    private readonly CreateCourierHandler _handler;

    public CreateCourierTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new CreateCourierHandler(database.Context);
    }

    [Fact]
    public async Task CreateValidCustomer_Should_PassValidation_And_BeCreated()
    {
        var command = new Command(
            "John",
            "Martin",
            "Doe",
            new[] { "whatever@gmail.com" });

        var response = await _handler.Handle(command, default);

        var result = response.Match(
            x => x.Courier,
            err => null!);

        result.Should().NotBeNull();
        result.Name.Should().Be("John Martin Doe");
        result.ContactInfo.Should().Contain("whatever@gmail.com");
    }
}