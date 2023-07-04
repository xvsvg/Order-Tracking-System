using Application.DataAccess.Contracts;
using Domain.Core.Implementations;
using Domain.Core.ValueObjects;
using FluentValidation;
using Infrastructure.DataAccess.Extensions;
using Infrastructure.Mapping.People;
using LanguageExt.Common;
using MediatR;
using static Application.Contracts.Customer.Commands.CreateCustomer;

namespace Application.Handlers.Customers;

internal class CreateCustomerHandler : IRequestHandler<Command, Result<Response>>
{
    private readonly IDatabaseContext _context;

    public CreateCustomerHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        var customer = new Customer(
            Guid.NewGuid(),
            new FullName(request.FirstName, request.MiddleName, request.LastName),
            request.ContactInfo.Select(x => new ContactInfo(x)).ToArray());

        var existingCustomer = await _context.Customers.FindExistingCustomerAsync(customer, cancellationToken);

        if (existingCustomer is not null)
        {
            var error = new ValidationException("User with provided data already exists");
            return new Result<Response>(error);
        }
        
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response(customer.ToDto());
    }
}