using Application.DataAccess.Contracts;
using Domain.Core.Implementations;
using Domain.Core.ValueObjects;
using Infrastructure.Mapping.People;
using MediatR;
using static Application.Contracts.Courier.Commands.UpdateCourier;

namespace Application.Handlers.Couriers;

internal class UpdateCourierHandler : IRequestHandler<Command, Response>
{
    private readonly IDatabaseContext _context;

    public UpdateCourierHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        var courier = new Courier(
            request.Id,
            new FullName(request.FirstName, request.MiddleName, request.LastName),
            request.ContactInfo.Select(x => new ContactInfo(x)).ToArray());

        _context.Couriers.Update(courier);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response(courier.ToDto());
    }
}