using Application.Dto;
using LanguageExt.Common;
using MediatR;

namespace Application.Contracts.Order.Commands;

internal static class CreateOrder
{
    public record Command(string Name, DateTime DispatchDate, DateTime? DeliveryDate, Guid? CourierId, Guid CustomerId) : IRequest<Result<Response>>;

    public record Response(OrderDto Order);
}