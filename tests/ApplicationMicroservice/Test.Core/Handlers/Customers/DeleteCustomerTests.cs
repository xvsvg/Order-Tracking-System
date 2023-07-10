using Application.Contracts.Customer.Commands;
using Application.Handlers.Customers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;

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
        var command = new DeleteCustomer.Command(customer.PersonId);

        var response = await _handler.Handle(command, default);

        var result = response.Match(
            x => customer,
            err => null!);

        result.Should().NotBeNull();
    }
}