using Application.Contracts.Validation;
using LanguageExt.Common;
using MediatR;

namespace Application.Contracts.Customer.Commands;

internal static class AddOrder
{
    public record Command(Guid OrderId, Guid CustomerId) : IValidatableRequest<Result<Unit>>;
}