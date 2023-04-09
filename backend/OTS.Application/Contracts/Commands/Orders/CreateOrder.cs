using MediatR;
using OTS.Application.Dto;

namespace OTS.Application.Contracts.Commands.Orders;

internal static class CreateOrder
{
    public record Command(DateTime DispatchDate, DateTime? DeliveryDate, Guid? CourierId) : IRequest<Response>;

    public record Response(OrderDto Order);
}