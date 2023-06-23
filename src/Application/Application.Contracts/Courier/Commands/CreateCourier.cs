using Application.Contracts.Validation;
using Application.Dto;
using LanguageExt.Common;

namespace Application.Contracts.Courier.Commands;

internal static class CreateCourier
{
    public record Command
        (string FirstName, string MiddleName, string LastName, IEnumerable<string> ContactInfo) : IValidatableRequest<Result<Response>>;

    public record Response(CourierDto Courier);
}