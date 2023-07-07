using Application.Handlers.Customers;
using FluentAssertions;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;
using static Application.Contracts.Customer.Commands.UpdateCustomer;

namespace Test.Core.Handlers.Customers;

public class UpdateCustomerTests : TestBase
{
    private readonly UpdateCustomerHandler _handler;

    public UpdateCustomerTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new UpdateCustomerHandler(database.Context);
    }

    [Fact]
    public async Task UpdateCustomer_Should_NotThrow()
    {
        var customer = await Database.Context.Customers.FirstAsync();
        var command = new Command(
            customer.PersonId,
            "John",
            "Martin",
            "Doe",
            customer.ContactInfo.Select(x => x.Contact));
        Database.Context.Entry(customer).State = EntityState.Detached;

        var response = await _handler.Handle(command, default);

        response.Customer.Name.Should().Be("John Martin Doe");
    }
}