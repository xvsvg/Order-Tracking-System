using Application.Common.Exceptions;
using Application.DataAccess.Contracts;
using Domain.Core.Implementations;
using Infrastructure.Mapping.Orders;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Contracts.Order.Commands.UpdateOrder;

namespace Application.Handlers.Orders;

internal class UpdateOrderHandler : IRequestHandler<Command, Result<Response>>
{
    private readonly IDatabaseContext _context;

    public UpdateOrderHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        var courier = await _context.Couriers
            .FirstOrDefaultAsync(x => x.PersonId.Equals(request.CourierId), cancellationToken);

        var customer = await _context.Customers
            .FirstOrDefaultAsync(x => x.PersonId.Equals(request.CustomerId), cancellationToken);

        if (customer is null)
        {
            var error = EntityNotFoundException.For<Customer>(request.CustomerId);
            return new Result<Response>(error);
        }

        var updatedOrder = new Order(
            request.Id,
            request.DispatchDate,
            request.DeliveryDate,
            courier,
            customer,
            request.Name);

        _context.Orders.Update(updatedOrder);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response(updatedOrder.ToDto());
    }
}