using FluentValidation;
using static Application.Contracts.Order.Queries.GetAllOrders;

namespace Application.Validation.Orders;

internal class GetAllOrdersValidator : AbstractValidator<Query>
{
    public GetAllOrdersValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than 0");
    }
}