using Application.Dto;
using LanguageExt.Common;
using MediatR;

namespace Application.Contracts.Order.Queries;

internal static class GetOrder
{
    public record Query(Guid Id) : IRequest<Result<Response>>;
    public record Response(OrderDto Order);
}