using LanguageExt.Common;
using MediatR;

namespace Application.Contracts.Courier.Commands;

internal static class RemoveOrder
{
    public record Command(Guid OrderId, Guid CourierId) : IRequest<Result<Unit>>;
}