using Application.Common.Exceptions;
using Application.DataAccess.Contracts;
using Domain.Core.Implementations;
using FluentValidation;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Contracts.Customer.Commands.DeleteCustomer;

namespace Application.Handlers.Customers;

internal class DeleteCustomerHandler : IRequestHandler<Command, Result<Unit>>
{
    private readonly IDatabaseContext _context;

    public DeleteCustomerHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(x => x.PersonId.Equals(request.Id), cancellationToken);

        if (customer is null)
        {
            var error = EntityNotFoundException.For<Customer>(request.Id);
            return new Result<Unit>(error);
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}