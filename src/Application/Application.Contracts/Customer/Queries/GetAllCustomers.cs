using Application.Dto.Pages;
using MediatR;

namespace Application.Contracts.Customer.Queries;

internal static class GetAllCustomers
{
    public record Query(int Page) : IRequest<Response>;
    public record Response(CustomerPageDto Page);
}