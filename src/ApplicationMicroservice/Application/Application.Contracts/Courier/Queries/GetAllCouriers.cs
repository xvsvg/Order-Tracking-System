using Application.Contracts.Validation;
using Application.Dto.Pages;

namespace Application.Contracts.Courier.Queries;

internal static class GetAllCouriers
{
    public record Query(int Page) : IValidatableRequest<Response>;

    public record Response(CourierPageDto Page);
}