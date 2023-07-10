using FastEndpoints;
using MediatR;
using static Application.Contracts.Courier.Commands.CreateCourier;

namespace Presentation.Endpoints.Couriers;

internal class CreateCourierEndpoint : Endpoint<Command, Response>
{
    private readonly IMediator _mediator;

    public CreateCourierEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("api/couriers");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await response.Match(
            x => SendCreatedAtAsync<GetCourierEndpoint>(
                new { x.Courier.Id }, x, generateAbsoluteUrl: true, cancellation: ct),
            err => SendErrorsAsync(cancellation: ct));
    }
}