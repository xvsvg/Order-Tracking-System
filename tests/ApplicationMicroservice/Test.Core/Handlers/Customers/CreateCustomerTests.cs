using Application.Handlers.Customers;
using FluentAssertions;
using Infrastructure.Seeding.Helpers;
using Test.Core.Fixtures;
using Xunit;
using static Application.Contracts.Customer.Commands.CreateCustomer;

namespace Test.Core.Handlers.Customers;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class CreateCustomerTests : IAsyncLifetime
{
    private readonly CoreDatabaseFixture _database;
    private readonly CreateCustomerHandler _handler;

    public CreateCustomerTests(CoreDatabaseFixture database)
    {
        _database = database;
        _handler = new CreateCustomerHandler(_database.Context);
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
            x => x.Customer,
            err => null!);

        result.Should().NotBeNull();
        result.Name.Should().Be("John Martin Doe");
        result.ContactInfo.Should().Contain("whatever@gmail.com");
    }
}