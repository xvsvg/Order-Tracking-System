using Application.Contracts.Validation;
using Application.Dto;

namespace Application.Contracts.Courier.Commands;

internal static class UpdateCourier
{
    public record Command
    (Guid Id, string FirstName, string MiddleName, string LastName,
        IEnumerable<string> ContactInfo) : IValidatableRequest<Response>;

    public record Response(CourierDto Courier);
}