using Application.Common.Exceptions;
using Application.DataAccess.Contracts;
using Domain.Core.Implementations;
using Infrastructure.Mapping.Orders;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Contracts.Order.Queries.GetOrder;

namespace Application.Handlers.Orders;

internal class GetOrderHandler : IRequestHandler<Query, Response>
{
    private readonly IDatabaseContext _context;

    public GetOrderHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(x => x.OrderId.Equals(request.Id), cancellationToken);

        var response = new Response(order?.ToDto());
        return response;
    }
}