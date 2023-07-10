using FastEndpoints;
using MediatR;
using static Application.Contracts.Courier.Commands.DeleteCourier;

namespace Presentation.Endpoints.Couriers;

internal class DeleteCourierEndpoint : Endpoint<Command>
{
    private readonly IMediator _mediator;

    public DeleteCourierEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("api/couriers");
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