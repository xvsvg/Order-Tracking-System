using Application.DataAccess.Contracts;
using Test.Endpoints.Fixtures;
using Xunit;

namespace Test.Endpoints;

[Collection(nameof(WebFactoryCollection))]
public class EndpointTestBase : IAsyncLifetime
{
    protected readonly HttpClient Client;
    protected readonly IDatabaseContext Database;
    private readonly Func<Task> Reset;

    protected EndpointTestBase(WebFactory factory)
    {
        Client = factory.Client;
        Database = factory.Context;
        Reset = factory.ResetAsync;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Reset();
    }
}