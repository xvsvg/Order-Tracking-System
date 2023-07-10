using Application.DataAccess.Contracts;
using Domain.Core.Implementations;
using Domain.Core.ValueObjects;
using Infrastructure.Mapping.People;
using MediatR;
using static Application.Contracts.Customer.Commands.UpdateCustomer;

namespace Application.Handlers.Customers;

internal class UpdateCustomerHandler : IRequestHandler<Command, Response>
{
    private readonly IDatabaseContext _context;

    public UpdateCustomerHandler(IDatabaseContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        var customer = new Customer(
            request.Id,
            new FullName(request.FirstName, request.MiddleName, request.LastName),
            request.ContactInfo.Select(x => new ContactInfo(x)).ToArray());

        _context.Customers.Update(customer);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response(customer.ToDto());
    }
}