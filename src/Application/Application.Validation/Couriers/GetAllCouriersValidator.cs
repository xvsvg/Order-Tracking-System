using FluentValidation;
using static Application.Contracts.Courier.Queries.GetAllCouriers;

namespace Application.Validation.Couriers;

internal class GetAllCouriersValidator : AbstractValidator<Query>
{
    public GetAllCouriersValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than 0");
    }
}