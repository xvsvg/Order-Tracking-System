using FastEndpoints;
using MediatR;
using Presentation.Endpoints.Tools;
using static Application.Contracts.Customer.Queries.GetCustomer;

namespace Presentation.Endpoints.Customers;

internal class GetCustomerEndpoint : Endpoint<Query, Response>
{
    private readonly IMediator _mediator;
    private readonly CacheConfiguration _cache;

    public GetCustomerEndpoint(IMediator mediator, CacheConfiguration cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    public override void Configure()
    {
        ResponseCache(_cache.TimeInSeconds);
        Verbs(Http.GET);
        Routes("api/customers/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Query req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        if (response.Customer is null)
            await SendNotFoundAsync(ct);
        else await SendOkAsync(response, cancellation: ct);
    }
}