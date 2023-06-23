using Application.Contracts.Validation;
using Application.Dto.Pages;
using LanguageExt.Common;

namespace Application.Contracts.Customer.Queries;

internal static class GetAllCustomers
{
    public record Query(int Page) : IValidatableRequest<Result<Response>>;
    public record Response(CustomerPageDto Page);
}