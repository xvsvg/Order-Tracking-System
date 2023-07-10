using Application.DataAccess.Contracts;
using Infrastructure.Mapping.People;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Contracts.Customer.Queries.GetCustomer;

namespace Application.Handlers.Customers;

internal class GetCustomerHandler : IRequestHandler<Query, Response>
{
    private readonly IDatabaseContext _context;

    public GetCustomerHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(x => x.PersonId.Equals(request.Id), cancellationToken);

        return new Response(customer?.ToDto());
    }
}