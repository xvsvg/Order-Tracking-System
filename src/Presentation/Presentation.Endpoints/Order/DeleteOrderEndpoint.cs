using FastEndpoints;
using MediatR;
using static Application.Contracts.Order.Commands.DeleteOrder;

namespace Presentation.Endpoints.Order;

internal class DeleteOrderEndpoint : Endpoint<Command>
{
    private readonly IMediator _mediator;

    public DeleteOrderEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("api/orders");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await response.Match(
            o => SendNoContentAsync(ct),
            e => SendErrorsAsync(cancellation: ct));
    }
}