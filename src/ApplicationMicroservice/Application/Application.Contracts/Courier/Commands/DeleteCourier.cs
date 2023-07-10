using LanguageExt.Common;
using MediatR;

namespace Application.Contracts.Courier.Commands;

internal static class DeleteCourier
{
    public record Command(Guid Id) : IRequest<Result<Unit>>;
}