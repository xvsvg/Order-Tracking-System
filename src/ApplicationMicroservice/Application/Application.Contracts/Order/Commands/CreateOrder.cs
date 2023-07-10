using Application.Contracts.Validation;
using Application.Dto;
using LanguageExt.Common;

namespace Application.Contracts.Order.Commands;

internal static class CreateOrder
{
    public record Command
    (string Name, DateTime DispatchDate, DateTime? DeliveryDate, Guid? CourierId,
        Guid CustomerId) : IValidatableRequest<Result<Response>>;

    public record Response(OrderDto Order);
}