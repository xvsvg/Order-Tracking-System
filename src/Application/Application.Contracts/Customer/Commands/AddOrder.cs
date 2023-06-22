using LanguageExt.Common;
using MediatR;

namespace Application.Contracts.Customer.Commands;

internal static class AddOrder
{
    public record Command(Guid OrderId, Guid CustomerId) : IRequest<Result<Unit>>;
}