using Application.Dto;
using Domain.Core.Implementations;

namespace Infrastructure.Mapping.Orders;

public static class OrderMapping
{
    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto(
            order.OrderId,
            order.Name,
            order.Customer.PersonId,
            order.Courier?.PersonId,
            order.DispatchDate,
            order.DeliveryDate);
    }
}