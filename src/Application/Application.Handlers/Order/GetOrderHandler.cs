using Application.Common.Exceptions;
using Application.DataAccess.Contracts;
using Infrastructure.Mapping.Orders;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Contracts.Order.Queries.GetOrder;

namespace Application.Handlers.Order;

internal class GetOrderHandler : IRequestHandler<Query, Result<Response>>
{
    private readonly IDatabaseContext _context;

    public GetOrderHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        var order  = await _context.Orders
            .FirstOrDefaultAsync(x => x.OrderId.Equals(request.Id), cancellationToken);

        if (order is null)
        {
            var error = EntityNotFoundException.For<Domain.Core.Implementations.Order>(request.Id);
            return new Result<Response>(error);
        }

        var response = new Response(order.ToDto());
        
        return new Result<Response>(response);
    }
}