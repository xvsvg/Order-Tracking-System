using Application.Common.Exceptions;
using Application.DataAccess.Contracts;
using Domain.Core.Implementations;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Contracts.Customer.Commands.RemoveOrder;

namespace Application.Handlers.Customers;

internal class RemoveOrderHandler : IRequestHandler<Command, Result<Unit>>
{
    private readonly IDatabaseContext _context;

    public RemoveOrderHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(x => x.PersonId.Equals(request.CustomerId), cancellationToken);

        var order = await _context.Orders
            .FirstOrDefaultAsync(x => x.OrderId.Equals(request.OrderId), cancellationToken);

        if (customer is null)
        {
            var error = EntityNotFoundException.For<Customer>(request.CustomerId);
            return new Result<Unit>(error);
        }

        if (order is null)
        {
            var error = EntityNotFoundException.For<Order>(request.OrderId);
            return new Result<Unit>(error);
        }

        customer.RemoveFromOrderHistory(order);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}