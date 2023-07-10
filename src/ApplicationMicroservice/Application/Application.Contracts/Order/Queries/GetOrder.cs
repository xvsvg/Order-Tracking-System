using Application.Dto;
using MediatR;

namespace Application.Contracts.Order.Queries;

internal static class GetOrder
{
    public record Query(Guid Id) : IRequest<Response>;

    public record Response(OrderDto? Order);
}