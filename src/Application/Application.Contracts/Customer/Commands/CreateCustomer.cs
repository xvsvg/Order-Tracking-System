using Application.Contracts.Validation;
using Application.Dto;
using LanguageExt.Common;
using MediatR;

namespace Application.Contracts.Customer.Commands;

internal static class CreateCustomer
{
    public record Command
        (string FirstName, string MiddleName, string LastName, IEnumerable<string> ContactInfo) : IValidatableRequest<Result<Response>>;

    public record Response(CustomerDto Customer);
}