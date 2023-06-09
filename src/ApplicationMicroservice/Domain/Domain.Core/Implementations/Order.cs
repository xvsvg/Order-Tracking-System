﻿namespace Domain.Core.Implementations;
#pragma warning disable CS8618

public class Order
{
    protected Order()
    {
    }

    public Order(
        Guid Id,
        DateTime dispatchDate,
        DateTime? deliveryDate,
        Courier? courier,
        Customer customer,
        string name)
    {
        ArgumentNullException.ThrowIfNull(dispatchDate);

        DispatchDate = dispatchDate;
        DeliveryDate = deliveryDate;
        Courier = courier;
        Customer = customer;
        Name = name;
        OrderId = Id;

        courier?.AddOrderToDeliver(this);
        customer.AddOrderToHistory(this);
    }

    public Guid OrderId { get; }
    public string Name { get; }
    public DateTime DispatchDate { get; }
    public DateTime? DeliveryDate { get; set; }
    public virtual Customer Customer { get; }
    public virtual Courier? Courier { get; set; }
}