using Application.DataAccess.Contracts;
using Domain.Core.Implementations;
using Domain.Core.ValueObjects;
using FluentValidation;
using Infrastructure.DataAccess.Extensions;
using Infrastructure.Mapping.People;
using LanguageExt.Common;
using MediatR;
using static Application.Contracts.Courier.Commands.CreateCourier;

namespace Application.Handlers.Couriers;

internal class CreateCourierHandler : IRequestHandler<Command, Result<Response>>
{
    private readonly IDatabaseContext _context;

    public CreateCourierHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        var courier = new Courier(
            Guid.NewGuid(),
            new FullName(request.FirstName, request.MiddleName, request.LastName),
            request.ContactInfo.Select(x => new ContactInfo(x)).ToArray());

        var existingCourier = await _context.Couriers.FindExistingPersonAsync(courier, cancellationToken);

        if (existingCourier is not null)
        {
            var error = new ValidationException("User with provided data already exists");
            return new Result<Response>(error);
        }

        _context.Couriers.Add(courier);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response(courier.ToDto());
    }
}