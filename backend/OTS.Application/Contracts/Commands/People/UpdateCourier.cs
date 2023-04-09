using MediatR;
using OTS.Application.Dto;

namespace OTS.Application.Contracts.Commands.People;

internal static class UpdateCourier
{
    public record Command(Guid CourierId, Guid CustomerId) : IRequest<Response>;

    public record Response(CourierDto Courier);
}