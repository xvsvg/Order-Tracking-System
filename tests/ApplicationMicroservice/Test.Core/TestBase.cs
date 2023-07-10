using Test.Core.Fixtures;
using Xunit;

namespace Test.Core;

[Collection(nameof(CoreDatabaseCollectionFixture))]
public class TestBase : IAsyncLifetime
{
    protected readonly CoreDatabaseFixture Database;
    private readonly Func<Task> Reset;

    protected TestBase(CoreDatabaseFixture database)
    {
        Database = database;
        Reset = database.ResetAsync;
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