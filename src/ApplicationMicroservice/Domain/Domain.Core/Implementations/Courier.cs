﻿using Domain.Core.Contracts;
using Domain.Core.ValueObjects;

#pragma warning disable CS8618

namespace Domain.Core.Implementations;

public class Courier : Person
{
    private readonly List<Order> _orders;

    protected Courier()
    {
        _orders = new List<Order>();
    }

    public Courier(
        Guid Id,
        FullName fullName,
        params ContactInfo[] contactInfo)
        : base(Id, fullName, contactInfo)
    {
        _orders = new List<Order>();
    }

    public virtual IEnumerable<Order> DeliveryList => _orders;

    public Order AddOrderToDeliver(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        _orders.Add(order);

        return order;
    }

    public void RemoveDeliveredOrder(Order order)
    {
        _orders.Remove(order);
    }
}