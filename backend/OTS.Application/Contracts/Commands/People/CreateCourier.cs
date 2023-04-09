using MediatR;
using OTS.Application.Dto;

namespace OTS.Application.Contracts.Commands.People;

internal static class CreateCourier
{
    public record Command(Guid? CustomerId, string Fullname, params string[] ContactInfo ) : IRequest<Response>;

    public record Response(CourierDto Courier);
}