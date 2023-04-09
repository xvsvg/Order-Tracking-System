using MediatR;
using OTS.Application.Dto;

namespace OTS.Application.Contracts.Commands.Orders;

internal static class UpdateOrder
{
    public record Command(Guid? CourierId, DateTime? DeliveryDate) : IRequest<Response>;

    public record Response(OrderDto Order);
}