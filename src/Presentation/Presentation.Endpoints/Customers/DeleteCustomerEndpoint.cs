using FastEndpoints;
using MediatR;
using static Application.Contracts.Customer.Commands.DeleteCustomer;

namespace Presentation.Endpoints.Customers;

internal class DeleteCustomerEndpoint : Endpoint<Command>
{
    private readonly IMediator _mediator;

    public DeleteCustomerEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("api/customers");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await response.Match(
            x => SendNoContentAsync(ct),
            err => SendErrorsAsync(cancellation: ct));
    }
}