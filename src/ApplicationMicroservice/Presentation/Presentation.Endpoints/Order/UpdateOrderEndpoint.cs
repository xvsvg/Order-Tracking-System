using FastEndpoints;
using MediatR;
using static Application.Contracts.Order.Commands.UpdateOrder;

namespace Presentation.Endpoints.Order;

internal class UpdateOrderEndpoint : Endpoint<Command, Response>
{
    private readonly IMediator _mediator;

    public UpdateOrderEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.PUT);
        Routes("api/orders");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await response.Match(
            x => SendOkAsync(x, ct),
            x => SendErrorsAsync(cancellation: ct));
    }
}