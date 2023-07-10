using FastEndpoints;
using MediatR;
using static Application.Contracts.Order.Commands.CreateOrder;

namespace Presentation.Endpoints.Order;

internal class CreateOrderEndpoint : Endpoint<Command, Response>
{
    private readonly IMediator _mediator;

    public CreateOrderEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("api/orders");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await response.Match(
            o => SendCreatedAtAsync<GetOrderEndpoint>(
                new { o.Order.Id }, o, generateAbsoluteUrl: true, cancellation: ct),
            err => SendErrorsAsync(cancellation: ct));
    }
}