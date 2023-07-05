using Application.Contracts.Tools;
using Application.DataAccess.Contracts;
using Application.Dto.Pages;
using Domain.Core.Implementations;
using Infrastructure.Mapping.People;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Contracts.Courier.Queries.GetAllCouriers;

namespace Application.Handlers.Couriers;

internal class GetAllCouriersHandler : IRequestHandler<Query, Response>
{
    private readonly IDatabaseContext _context;
    private readonly int _pageCount;

    public GetAllCouriersHandler(IDatabaseContext context, PaginationConfiguration configuration)
    {
        _context = context;
        _pageCount = configuration.PageSize;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        IQueryable<Courier> query = _context.Couriers;

        var customerCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)customerCount / _pageCount);

        var couriers = await query
            .OrderBy(x => x.FullName)
            .Skip((request.Page - 1) * _pageCount)
            .Take(_pageCount)
            .ToListAsync(cancellationToken);

        var page = new CourierPageDto(couriers.Select(x => x?.ToDto()), request.Page, totalPages);
        return new Response(page);
    }
}