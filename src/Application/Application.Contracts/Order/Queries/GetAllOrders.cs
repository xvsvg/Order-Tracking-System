using Application.Dto.Pages;
using MediatR;

namespace Application.Contracts.Order.Queries;

internal static class GetAllOrders
{
    public record Query(int Page) : IRequest<Response>;
    public record Response(OrderPageDto Page);
}