using LanguageExt.Common;
using MediatR;

namespace Application.Contracts.Customer.Commands;

internal static class DeleteCustomer
{
    public record Command(Guid Id) : IRequest<Result<Unit>>;
}