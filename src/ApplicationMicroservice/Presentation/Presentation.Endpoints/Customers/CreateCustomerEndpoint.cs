using FastEndpoints;
using MediatR;
using static Application.Contracts.Customer.Commands.CreateCustomer;

namespace Presentation.Endpoints.Customers;

internal class CreateCustomerEndpoint : Endpoint<Command, Response>
{
    private readonly IMediator _mediator;

    public CreateCustomerEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("api/customers");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await response.Match(
            x => SendCreatedAtAsync<GetCustomerEndpoint>(
                new { x.Customer.Id }, x, generateAbsoluteUrl: true, cancellation: ct),
            err => SendErrorsAsync(cancellation: ct));
    }
}