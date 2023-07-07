using Application.Handlers.Customers;
using FluentAssertions;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;
using static Application.Contracts.Customer.Commands.DeleteCustomer;

namespace Test.Core.Handlers.Customers;

public class DeleteCustomerTests : TestBase
{
    private readonly DeleteCustomerHandler _handler;

    public DeleteCustomerTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new DeleteCustomerHandler(database.Context);
    }

    [Fact]
    public async Task DeleteCustomer_Should_SuccessfullyDelete()
    {
        var customer = await Database.Context.Customers.FirstAsync();
        var command = new Command(customer.PersonId);

        var response = await _handler.Handle(command, default);

        var result = response.Match(
            x => customer,
            err => null!);

        result.Should().NotBeNull();
    }
}