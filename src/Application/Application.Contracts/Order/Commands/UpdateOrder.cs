using Application.Dto;
using LanguageExt.Common;
using MediatR;

namespace Application.Contracts.Order.Commands;

internal static class UpdateOrder
{
    public record Command(Guid Id, string Name, DateTime DispatchDate, DateTime? DeliveryDate, Guid? CourierId, Guid CustomerId) : IRequest<Result<Response>>;

    public record Response(OrderDto Order);
}