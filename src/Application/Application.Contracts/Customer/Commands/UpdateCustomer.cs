using Application.Contracts.Validation;
using Application.Dto;

namespace Application.Contracts.Customer.Commands;

internal static class UpdateCustomer
{
    public record Command
        (Guid Id, string FirstName, string MiddleName, string LastName, IEnumerable<string> ContactInfo) : IValidatableRequest<Response>;

    public record Response(CustomerDto Customer);
}