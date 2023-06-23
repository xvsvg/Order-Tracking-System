using Application.Dto;
using MediatR;

namespace Application.Contracts.Courier.Queries;

internal static class GetCourier
{
    public record Query(Guid Id) : IRequest<Response>;
    public record Response(CourierDto? Courier);
}