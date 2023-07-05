using FastEndpoints;
using MediatR;
using static Application.Contracts.Courier.Commands.AddOrder;

namespace Presentation.Endpoints.Couriers;

internal class AddOrderEndpoint : Endpoint<Command>
{
    private readonly IMediator _mediator;

    public AddOrderEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.PUT);
        Routes("api/couriers/{courierId:guid}/orders/{orderId:guid}");
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