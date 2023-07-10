using System.Data.Common;
using Infrastructure.DataAccess.DatabaseContexts;
using Infrastructure.DataAccess.Extensions;
using Infrastructure.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Test.Core.Fixtures;

public class CoreDatabaseFixture : DatabaseFixture
{
    public DatabaseContext Context { get; private set; } = null!;
    public AsyncServiceScope Scope { get; private set; }

    protected override void ConfigureServices(IServiceCollection collection)
    {
        collection.AddDatabaseContext(x =>
            x.UseLazyLoadingProxies().UseNpgsql(Container.GetConnectionString()));
    }

    public override async Task ResetAsync()
    {
        await base.ResetAsync();
        Context.ChangeTracker.Clear();
        await SeedingHelper.SeedDatabaseAsync(Context);
    }

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await Scope.DisposeAsync();
    }

    protected override DbConnection CreateConnection()
    {
        return Context.Database.GetDbConnection();
    }

    protected override Task UseProviderAsync(IServiceProvider provider)
    {
        Scope = provider.CreateAsyncScope();

        Context = Scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        return Task.CompletedTask;
    }

    protected override async Task SeedDatabaseAsync()
    {
        await SeedingHelper.SeedDatabaseAsync(Context);
    }
}