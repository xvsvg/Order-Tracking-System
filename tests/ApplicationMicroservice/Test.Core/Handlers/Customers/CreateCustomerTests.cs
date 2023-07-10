﻿using Application.Contracts.Customer.Commands;
using Application.Handlers.Customers;
using FluentAssertions;
using Test.Core.Fixtures;
using Xunit;

namespace Test.Core.Handlers.Customers;

public class CreateCustomerTests : TestBase
{
    private readonly CreateCustomerHandler _handler;

    public CreateCustomerTests(CoreDatabaseFixture database) : base(database)
    {
        _handler = new CreateCustomerHandler(database.Context);
    }

    [Fact]
    public async Task CreateValidCustomer_Should_PassValidation_And_BeCreated()
    {
        var command = new CreateCustomer.Command(
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