using Application.Common.Exceptions;
using Application.DataAccess.Contracts;
using Domain.Core.Implementations;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Contracts.Courier.Commands.DeleteCourier;

namespace Application.Handlers.Couriers;

internal class DeleteCourierHandler : IRequestHandler<Command, Result<Unit>>
{
    private readonly IDatabaseContext _context;

    public DeleteCourierHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
    {
        var courier = await _context.Couriers
            .FirstOrDefaultAsync(x => x.PersonId.Equals(request.Id), cancellationToken);

        if (courier is null)
        {
            var error = EntityNotFoundException.For<Courier>(request.Id);
            return new Result<Unit>(error);
        }

        _context.Couriers.Remove(courier);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}