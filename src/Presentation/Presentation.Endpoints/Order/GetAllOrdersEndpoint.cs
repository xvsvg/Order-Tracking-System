using System.Net;
using FastEndpoints;
using FluentValidation;
using MediatR;
using Presentation.Endpoints.Tools;
using static Application.Contracts.Order.Queries.GetAllOrders;

namespace Presentation.Endpoints.Order;

internal class GetAllOrdersEndpoint : Endpoint<Query, Response>
{
    private readonly CacheConfiguration _cacheConfiguration;
    private readonly IMediator _mediator;

    public GetAllOrdersEndpoint(CacheConfiguration cacheConfiguration, IMediator mediator)
    {
        _cacheConfiguration = cacheConfiguration;
        _mediator = mediator;
    }

    public override void Configure()
    {
        ResponseCache(_cacheConfiguration.TimeInSeconds, varyByQueryKeys: new[] { "page" });
        Verbs(Http.GET);
        Routes("api/orders");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Query req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        await response.Match(
            o => SendAsync(o, statusCode: (int)HttpStatusCode.PartialContent, cancellation: ct),
            err =>
            {
                if (err is ValidationException ex)
                    return SendErrorsAsync(cancellation: ct);
                        
                return SendNoContentAsync(cancellation: ct);
            });
    }
}