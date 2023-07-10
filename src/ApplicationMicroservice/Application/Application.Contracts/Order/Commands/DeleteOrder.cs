using LanguageExt.Common;
using MediatR;

namespace Application.Contracts.Order.Commands;

internal static class DeleteOrder
{
    public record Command(Guid Id) : IRequest<Result<Unit>>;
}