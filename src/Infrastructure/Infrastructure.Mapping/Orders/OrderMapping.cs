using Application.Dto;
using Domain.Core.Implementations;
using Infrastructure.Mapping.People;

namespace Infrastructure.Mapping.Orders;

public static class OrderMapping
{
    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto(
            order.OrderId,
            order.Customer.PersonId,
            order.DispatchDate,
            order.DeliveryDate,
            order.Courier?.PersonId);
    }
}