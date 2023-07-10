using Application.Contracts.Courier.Commands;
using Application.Handlers.Couriers;
using FluentAssertions;
using Test.Core.Fixtures;
using Xunit;

namespace Test.Core.Handlers.Couriers;

public class CreateCourierTests : TestBase
{
    private readonly CreateCourierHandler _handler;

    public CreateCourierTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new CreateCourierHandler(database.Context);
    }

    [Fact]
    public async Task CreateValidCourier_Should_PassValidation_And_BeCreated()
    {
        var command = new CreateCourier.Command(
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