﻿using System.Net;
using Application.Contracts.Order.Commands;
using Domain.Core.Implementations;
using FastEndpoints;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Order;
using Test.Endpoints.Fixtures;
using Xunit;

namespace Test.Endpoints.Orders;

public class CreateOrderEndpointTests : EndpointTestBase
{
    private readonly Lazy<Task<Customer>> _customer;

    public CreateOrderEndpointTests(WebFactory factory) : base(factory)
    {
        _customer = new Lazy<Task<Customer>>(async () =>
            await Database.Customers.FirstAsync());
    }

    [Fact]
    public async Task CreateValidOrder_Should_PassValidation()
    {
        var command = new CreateOrder.Command(
            "Whatever",
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(2),
            null,
            _customer.Value.Result.PersonId);

        var (response, result) = await Client
            .POSTAsync<CreateOrderEndpoint, CreateOrder.Command, CreateOrder.Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Should().NotBeNull();
        result!.Order.CourierId.Should().Be(null);
        result.Order.CustomerId.Should().Be(_customer.Value.Result.PersonId);
        result.Order.Name.Should().Be("Whatever");
    }

    [Fact]
    public async Task CreateInvalidOrder_Should_NotPassValidation()
    {
        var command = new CreateOrder.Command(
            "djsklajd123jilkfg[",
            DateTime.UtcNow,
            DateTime.UtcNow,
            null,
            _customer.Value.Result.PersonId);

        var (response, result) = await Client
            .POSTAsync<CreateOrderEndpoint, CreateOrder.Command, CreateOrder.Response>(command);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeNull();
        result!.Order.Should().BeNull();
    }
}