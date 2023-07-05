using System.Net;
using FastEndpoints;
using MediatR;
using Presentation.Endpoints.Tools;
using static Application.Contracts.Courier.Queries.GetAllCouriers;

namespace Presentation.Endpoints.Couriers;

internal class GetAllCouriersEndpoint : Endpoint<Query, Response>
{
    private readonly IMediator _mediator;
    private readonly CacheConfiguration _cache;

    public GetAllCouriersEndpoint(IMediator mediator, CacheConfiguration cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    public override void Configure()
    {
        ResponseCache(_cache.TimeInSeconds, varyByQueryKeys: new[] { "page" });
        Verbs(Http.GET);
        Routes("api/couriers");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Query req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        if (response.Page.Couriers.Any())
            await SendAsync(response, statusCode: (int)HttpStatusCode.PartialContent, cancellation: ct);
        else await SendNotFoundAsync(ct);
    }
}