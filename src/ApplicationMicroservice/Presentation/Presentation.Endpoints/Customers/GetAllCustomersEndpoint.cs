using System.Net;
using FastEndpoints;
using MediatR;
using Presentation.Endpoints.Tools;
using static Application.Contracts.Customer.Queries.GetAllCustomers;

namespace Presentation.Endpoints.Customers;

internal class GetAllCustomersEndpoint : Endpoint<Query, Response>
{
    private readonly CacheConfiguration _cacheConfiguration;
    private readonly IMediator _mediator;

    public GetAllCustomersEndpoint(IMediator mediator, CacheConfiguration cacheConfiguration)
    {
        _mediator = mediator;
        _cacheConfiguration = cacheConfiguration;
    }

    public override void Configure()
    {
        ResponseCache(_cacheConfiguration.TimeInSeconds, varyByQueryKeys: new[] { "page" });
        Verbs(Http.GET);
        Routes("api/customers");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Query req, CancellationToken ct)
    {
        var response = await _mediator.Send(req, ct);

        if (response.Page.Customers.Any())
            await SendAsync(response, (int)HttpStatusCode.PartialContent, ct);
        else await SendNoContentAsync(ct);
    }
}