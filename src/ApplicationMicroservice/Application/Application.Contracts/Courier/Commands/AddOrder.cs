using Application.Contracts.Validation;
using LanguageExt.Common;
using MediatR;

namespace Application.Contracts.Courier.Commands;

internal static class AddOrder
{
    public record Command(Guid OrderId, Guid CourierId) : IValidatableRequest<Result<Unit>>;
}