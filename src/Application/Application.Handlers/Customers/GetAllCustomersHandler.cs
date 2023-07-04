using Application.Contracts.Tools;
using Application.DataAccess.Contracts;
using Application.Dto.Pages;
using Domain.Core.Implementations;
using Infrastructure.Mapping.People;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Contracts.Customer.Queries.GetAllCustomers;

namespace Application.Handlers.Customers;

internal class GetAllCustomersHandler : IRequestHandler<Query, Response>
{
    private readonly IDatabaseContext _context;
    private readonly int _pageCount;

    public GetAllCustomersHandler(IDatabaseContext context, PaginationConfiguration paginationConfiguration)
    {
        _context = context;
        _pageCount = paginationConfiguration.PageSize;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        IQueryable<Customer> query = _context.Customers;

        var customerCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)customerCount / _pageCount);

        var customers = await query
            .OrderBy(x => x.FullName)
            .Skip((request.Page - 1) * _pageCount)
            .Take(_pageCount)
            .ToListAsync(cancellationToken);

        var page = new CustomerPageDto(customers.Select(x => x?.ToDto()), request.Page, totalPages);
        return new Response(page);
    }
}