﻿using FluentValidation;
using static Application.Contracts.Customer.Commands.CreateCustomer;

namespace Application.Validation.Customers;

internal class CreateCustomerValidator : AbstractValidator<Command>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().MinimumLength(3).Matches("^\\p{Lu}\\p{Ll}*$")
            .WithMessage("First name must be at least 3 characters length and start with upper case letter");

        RuleFor(x => x.MiddleName)
            .NotEmpty().MinimumLength(3).Matches("^\\p{Lu}\\p{Ll}*$")
            .WithMessage("Middle name must be at least 3 characters length and start with upper case letter");

        RuleFor(x => x.FirstName)
            .NotEmpty().MinimumLength(3).Matches("^\\p{Lu}\\p{Ll}*$")
            .WithMessage("Last name must be at least 3 characters length and start with upper case letter");

        RuleFor(x => x.ContactInfo)
            .NotEmpty()
            .WithMessage("User must have contact information");
    }
}