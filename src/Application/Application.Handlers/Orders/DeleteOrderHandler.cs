using Application.Common.Exceptions;
using Application.DataAccess.Contracts;
using Domain.Core.Implementations;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Contracts.Order.Commands.DeleteOrder;

namespace Application.Handlers.Orders;

internal class DeleteOrderHandler : IRequestHandler<Command, Result<Unit>>
{
    private readonly IDatabaseContext _context;

    public DeleteOrderHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(x => x.OrderId.Equals(request.Id), cancellationToken);

        if (order is null)
        {
            var error = EntityNotFoundException.For<Order>(request.Id);
            return new Result<Unit>(error);
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}