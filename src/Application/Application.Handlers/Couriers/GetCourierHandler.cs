using Application.DataAccess.Contracts;
using Infrastructure.Mapping.People;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Contracts.Courier.Queries.GetCourier;

namespace Application.Handlers.Couriers;

internal class GetCourierHandler : IRequestHandler<Query, Response>
{
    private readonly IDatabaseContext _context;

    public GetCourierHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var courier = await _context.Couriers
            .FirstOrDefaultAsync(x => x.PersonId.Equals(request.Id), cancellationToken);

        return new Response(courier?.ToDto());
    }
}