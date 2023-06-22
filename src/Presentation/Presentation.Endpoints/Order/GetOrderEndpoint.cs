using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using static Application.Contracts.Order.Queries.GetOrder;

namespace Presentation.Endpoints.Order;

[HttpGet("api/orders", "api/orders/{id:guid}")]
[AllowAnonymous]
internal class GetOrderEndpoint : Endpoint<Query, Response>
{
    private readonly IMediator _mediator;

    public GetOrderEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task HandleAsync(Query req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await response.Match(
            o => SendOkAsync(o, cancellation: ct),
            err => SendNotFoundAsync(cancellation: ct));
    }
}