using FastEndpoints;
using MediatR;
using static Application.Contracts.Courier.Commands.RemoveOrder;

namespace Presentation.Endpoints.Couriers;

internal class RemoveOrderEndpoint : Endpoint<Command>
{
    private readonly IMediator _mediator;

    public RemoveOrderEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("api/couriers/{customerId:guid}/orders/{orderId:guid}");
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