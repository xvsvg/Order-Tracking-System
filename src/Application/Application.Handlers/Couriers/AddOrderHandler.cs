using Application.Common.Exceptions;
using Application.DataAccess.Contracts;
using Domain.Core.Implementations;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Contracts.Courier.Commands.AddOrder;

namespace Application.Handlers.Couriers;

internal class AddOrderHandler : IRequestHandler<Command, Result<Unit>>
{
    private readonly IDatabaseContext _context;

    public AddOrderHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
    {
        var courier = await _context.Couriers
            .FirstOrDefaultAsync(x => x.PersonId.Equals(request.CourierId), cancellationToken);

        var order = await _context.Orders
            .FirstOrDefaultAsync(x => x.OrderId.Equals(request.OrderId), cancellationToken);

        if (order is null)
        {
            var error = EntityNotFoundException.For<Order>(request.OrderId);
            return new Result<Unit>(error);
        }
        
        if (courier is null)
        {
            var error = EntityNotFoundException.For<Courier>(request.OrderId);
            return new Result<Unit>(error);
        }
        
        courier.RemoveDeliveredOrder(order);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}