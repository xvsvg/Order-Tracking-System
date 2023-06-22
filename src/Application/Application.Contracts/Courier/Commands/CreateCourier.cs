using Application.Dto;
using LanguageExt.Common;
using MediatR;

namespace Application.Contracts.Courier.Commands;

internal static class CreateCourier
{
    public record Command
        (string FirstName, string MiddleName, string LastName, IEnumerable<string> ContactInfo) : IRequest<Result<Response>>;

    public record Response(CourierDto Courier);
}