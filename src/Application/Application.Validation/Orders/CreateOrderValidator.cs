using FluentValidation;
using static Application.Contracts.Order.Commands.CreateOrder;

namespace Application.Validation.Orders;

internal class CreateOrderValidator : AbstractValidator<Command>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().MinimumLength(3).Matches("^\\p{Lu}\\p{Ll}*$")
            .WithMessage("Product name should start with upper case and at least 3 characters length");

        RuleFor(x => x.CourierId)
            .Must(x => !x.HasValue || !x.Value.Equals(Guid.Empty))
            .WithMessage("Courier id is invalid");
        
        RuleFor(x => x.CustomerId)
            .Must(x => !x.Equals(Guid.Empty))
            .WithMessage("Customer id is invalid");

        RuleFor(x => x.DeliveryDate)
            .Must(x => !x.HasValue || x.Value >= DateTime.Now)
            .WithMessage("Delivery date should not be earlier than now");

        RuleFor(x => x.DispatchDate)
            .Must(x => x >= DateTime.Now)
            .WithMessage("Dispatch date should not be earlier than now");
    }
}