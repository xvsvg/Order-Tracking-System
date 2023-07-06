using Application.Handlers.Customers;
using FluentAssertions;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;
using static Application.Contracts.Customer.Commands.DeleteCustomer;

namespace Test.Core.Handlers.Customers;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class DeleteCustomerTests : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;
    private readonly DeleteCustomerHandler _handler;

    public DeleteCustomerTests(CoreDatabaseFixture database)
    {
        _database = database;
        _handler = new DeleteCustomerHandler(_database.Context);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _database.ResetAsync();
    }

    [Fact]
    public async Task DeleteCustomer_ShouldNotThrow()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);
        var customer = await _database.Context.Customers.FirstAsync();
        var command = new Command(customer.PersonId);

        var response = await _handler.Handle(command, default);

        var result = response.Match(
            x => customer,
            err => null!);

        result.Should().NotBeNull();
    }
}