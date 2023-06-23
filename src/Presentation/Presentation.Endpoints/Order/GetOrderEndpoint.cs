using FastEndpoints;
using MediatR;
using Presentation.Endpoints.Tools;
using static Application.Contracts.Order.Queries.GetOrder;

namespace Presentation.Endpoints.Order;

internal class GetOrderEndpoint : Endpoint<Query, Response>
{
    private readonly IMediator _mediator;
    private readonly CacheConfiguration _cacheConfiguration;

    public GetOrderEndpoint(IMediator mediator, CacheConfiguration cacheConfiguration)
    {
        _mediator = mediator;
        _cacheConfiguration = cacheConfiguration;
    }

    public override void Configure()
    {
        ResponseCache(_cacheConfiguration.TimeInSeconds);
        Verbs(Http.GET);
        Routes("api/orders/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Query req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        if (response.Order is null)
            await SendNotFoundAsync(ct);

        await SendOkAsync(response, ct);
    }
}