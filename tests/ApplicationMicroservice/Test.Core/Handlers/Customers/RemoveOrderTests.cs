﻿using Application.Handlers.Customers;
using FluentAssertions;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Test.Core.Fixtures;
using Xunit;
using static Application.Contracts.Customer.Commands.RemoveOrder;

namespace Test.Core.Handlers.Customers;

public class RemoveOrderTests : TestBase
{
    private readonly RemoveOrderHandler _handler;

    public RemoveOrderTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new RemoveOrderHandler(database.Context);
    }

    [Fact]
    public async Task RemoveOrder_Should_ThrowIfOrderExists()
    {
        var customer = await Database.Context.Customers.FirstOrDefaultAsync(x => x.OrderHistory.Any());
        var order = customer!.OrderHistory.First();
        var command = new Command(customer.PersonId, order.OrderId);

        var response = await _handler.Handle(command, default);

        var act = async () =>
        {
            return await Database.Context.Orders.FirstAsync(x => x.OrderId.Equals(order.OrderId));
        };

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}