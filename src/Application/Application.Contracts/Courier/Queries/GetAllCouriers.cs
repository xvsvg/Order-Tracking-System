using Application.Dto.Pages;
using MediatR;

namespace Application.Contracts.Courier.Queries;

internal static class GetAllCouriers
{
    public record Query(int Page) : IRequest<Response>;
    public record Response(CustomerPageDto Page);
}