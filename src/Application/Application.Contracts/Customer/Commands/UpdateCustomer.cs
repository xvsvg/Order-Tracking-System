using Application.Dto;
using LanguageExt.Common;
using MediatR;

namespace Application.Contracts.Customer.Commands;

internal static class UpdateCustomer
{
    public record Command
        (Guid Id, string FirstName, string MiddleName, string LastName, IEnumerable<string> ContactInfo) : IRequest<Result<Response>>;

    public record Response(CustomerDto Customer);
}