using Application.Contracts.Tools;
using Application.DataAccess.Contracts;
using Application.Dto.Pages;
using Domain.Core.Implementations;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Mapping.Orders;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Contracts.Order.Queries.GetAllOrders;

namespace Application.Handlers.Orders;

internal class GetAllOrdersHandler : IRequestHandler<Query, Response>
{
    private readonly IDatabaseContext _context;
    private readonly int _pageCount;

    public GetAllOrdersHandler(IDatabaseContext context, PaginationConfiguration paginationConfiguration)
    {
        _context = context;
        _pageCount = paginationConfiguration.PageSize;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        IQueryable<Order> query = _context.Orders;

        var ordersCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)ordersCount / _pageCount);

        var orders = await query
            .OrderBy(x => x.Name)
            .Skip((request.Page - 1) * _pageCount)
            .Take(_pageCount)
            .ToListAsync(cancellationToken);

        var page = new OrderPageDto(orders.Select(x => x?.ToDto()), request.Page, totalPages);
        return new Response(page);
    }
}