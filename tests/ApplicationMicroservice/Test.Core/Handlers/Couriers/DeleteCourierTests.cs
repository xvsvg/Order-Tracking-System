using Application.Handlers.Couriers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Test.Tools.Helpers;
using Xunit;
using static Application.Contracts.Courier.Commands.DeleteCourier;

namespace Test.Core.Handlers.Couriers;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class DeleteCourierTests : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;
    private readonly DeleteCourierHandler _handler;

    public DeleteCourierTests(CoreDatabaseFixture database)
    {
        _database = database;
        _handler = new DeleteCourierHandler(_database.Context);
    }

    [Fact]
    public async Task DeleteCustomer_ShouldNotThrow()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);
        var courier = await _database.Context.Couriers.FirstAsync();
        var command = new Command(courier.PersonId);
        
        var response = await _handler.Handle(command, default);

        var result = response.Match(
            x => courier,
            err => null!);

        result.Should().NotBeNull();
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