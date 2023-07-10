using FluentValidation;
using static Application.Contracts.Order.Commands.UpdateOrder;

namespace Application.Validation.Orders;

internal class UpdateOrderValidator : AbstractValidator<Command>
{
    public UpdateOrderValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().MinimumLength(3).Matches("^\\p{Lu}\\p{Ll}*$")
            .WithMessage("Order name should be at least 3 characters length and start with upper case");

        RuleFor(x => x.CourierId)
            .Must(x => !x.HasValue || !x.Value.Equals(Guid.Empty))
            .WithMessage("Courier id is invalid");

        RuleFor(x => x.CustomerId)
            .Must(x => !x.Equals(Guid.Empty))
            .WithMessage("Customer id is invalid");
    }
}