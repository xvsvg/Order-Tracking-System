using FastEndpoints;
using MediatR;
using static Application.Contracts.Customer.Commands.UpdateCustomer;

namespace Presentation.Endpoints.Customers;

internal class UpdateCustomerEndpoint : Endpoint<Command, Response>
{
    private readonly IMediator _mediator;

    public UpdateCustomerEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.PUT);
        Routes("api/customers");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendNoContentAsync(ct);
    }
}