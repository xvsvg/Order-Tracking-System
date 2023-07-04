using Application.Contracts.Validation;
using Application.Dto.Pages;
using LanguageExt.Common;

namespace Application.Contracts.Order.Queries;

internal static class GetAllOrders
{
    public record Query(int Page) : IValidatableRequest<Response>;
    public record Response(OrderPageDto Page);
}