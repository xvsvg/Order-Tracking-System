using FluentValidation;
using static Application.Contracts.Customer.Queries.GetAllCustomers;

namespace Application.Validation.Customers;

internal class GetAllCustomersValidator : AbstractValidator<Query>
{
    public GetAllCustomersValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than 0");
    }
}