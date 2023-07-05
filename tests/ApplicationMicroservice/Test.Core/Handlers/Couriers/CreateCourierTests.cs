using Application.Handlers.Couriers;
using FluentAssertions;
using Test.Core.Fixtures;
using Test.Tools.Helpers;
using Xunit;
using static Application.Contracts.Courier.Commands.CreateCourier;

namespace Test.Core.Handlers.Couriers;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class CreateCourierTests : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;
    private readonly CreateCourierHandler _handler;

    public CreateCourierTests(CoreDatabaseFixture database)
    {
        _database = database;
        _handler = new CreateCourierHandler(_database.Context);
    }

    [Fact]
    public async Task CreateValidCustomer_ShouldPassValidation_And_BeCreated()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);
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

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _database.ResetAsync();
    }
}