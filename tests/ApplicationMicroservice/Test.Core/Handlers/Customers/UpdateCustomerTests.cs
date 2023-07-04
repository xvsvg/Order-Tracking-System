using Application.Handlers.Customers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Test.Tools.Helpers;
using Xunit;
using static Application.Contracts.Customer.Commands.UpdateCustomer;

namespace Test.Core.Handlers.Customers;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class UpdateCustomerTests : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;
    private readonly UpdateCustomerHandler _handler;

    public UpdateCustomerTests(CoreDatabaseFixture database)
    {
        _database = database;
        _handler = new UpdateCustomerHandler(_database.Context);
    }

    [Fact]
    public async Task UpdateCustomer_Should_NotThrow()
    {
        await SeedingHelper.SeedDatabaseAsync(_database.Context);
        var customer = await _database.Context.Customers.FirstAsync();

        var command = new Command(
            customer.PersonId,
            "John",
            "Martin",
            "Doe",
            customer.ContactInfo.Select(x => x.Contact));
        _database.Context.Entry(customer).State = EntityState.Detached;
        
        var response = await _handler.Handle(command, default);

        response.Customer.Name.Should().Be("John Martin Doe");
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