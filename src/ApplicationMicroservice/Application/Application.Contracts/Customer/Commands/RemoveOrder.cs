using LanguageExt.Common;
using MediatR;

namespace Application.Contracts.Customer.Commands;

internal static class RemoveOrder
{
    public record Command(Guid CustomerId, Guid OrderId) : IRequest<Result<Unit>>;
}