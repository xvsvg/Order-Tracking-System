using Application.Handlers.Couriers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Test.Tools.Helpers;
using Xunit;
using static Application.Contracts.Courier.Commands.UpdateCourier;

namespace Test.Core.Handlers.Couriers;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class UpdateCourierTests : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;
    private readonly UpdateCourierHandler _handler;

    public UpdateCourierTests(CoreDatabaseFixture database)
    {
        _database = database;
        _handler = new UpdateCourierHandler(_database.Context);
    }

    [Fact]
    public async Task UpdateCustomer_Should_NotThrow()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);
        var courier = await _database.Context.Couriers.FirstAsync();

        var command = new Command(
            courier.PersonId,
            "John",
            "Martin",
            "Doe",
            courier.ContactInfo.Select(x => x.Contact));
        _database.Context.Entry(courier).State = EntityState.Detached;

        var response = await _handler.Handle(command, default);

        response.Courier.Name.Should().Be("John Martin Doe");
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