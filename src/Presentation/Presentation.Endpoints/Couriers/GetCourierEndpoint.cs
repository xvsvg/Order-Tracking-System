using FastEndpoints;
using MediatR;
using static Application.Contracts.Courier.Queries.GetCourier;

namespace Presentation.Endpoints.Couriers;

internal class GetCourierEndpoint : Endpoint<Query, Response>
{
    private readonly IMediator _mediator;

    public GetCourierEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("api/couriers/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Query req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        if (response.Courier is null)
            await SendNotFoundAsync(ct);
        else await SendOkAsync(response, cancellation: ct);
    }
}