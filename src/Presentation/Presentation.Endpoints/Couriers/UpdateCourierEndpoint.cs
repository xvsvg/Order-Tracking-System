﻿using FastEndpoints;
using MediatR;
using static Application.Contracts.Courier.Commands.UpdateCourier;

namespace Presentation.Endpoints.Couriers;

internal class UpdateCourierEndpoint : Endpoint<Command, Response>
{
    private readonly IMediator _mediator;

    public UpdateCourierEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.PUT);
        Routes("api/couriers");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}