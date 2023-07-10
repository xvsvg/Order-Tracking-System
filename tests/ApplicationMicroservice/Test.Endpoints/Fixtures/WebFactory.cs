using System.Data;
using System.Data.Common;
using Application.Validation.Extensions;
using FastEndpoints;
using Infrastructure.DataAccess;
using Infrastructure.DataAccess.DatabaseContexts;
using Infrastructure.Seeding.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Playground.Web;
using Respawn;
using Test.Tools.Extensions;
using Testcontainers.PostgreSql;
using Xunit;

namespace Test.Endpoints.Fixtures;

public class WebFactory : WebApplicationFactory<IDevelopmentEnvironmentMarker>, IAsyncLifetime
{
    private const string User = "postgres";
    private const string Password = "postgres";
    private const string Database = "postgres";
    private Respawner _respawner = null!;

    public WebFactory()
    {
        Container = new PostgreSqlBuilder()
            .WithUsername(User)
            .WithPassword(Password)
            .WithDatabase(Database)
            .Build();
    }

    public HttpClient Client { get; private set; } = default!;
    public DbConnection Connection { get; private set; } = default!;
    public PostgreSqlContainer Container { get; }
    public DatabaseContext Context { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
        await InitializeContextAsync();
        await CreateConnectionAsync();
        await SeedingHelper.SeedDatabaseAsync(Context);
        await InitializeRespawnerAsync();
        Client = CreateClient();
    }

    public new async Task DisposeAsync()
    {
        await Context.DisposeAsync();
        await Container.StopAsync();
        await Connection.DisposeAsync();
        Client.Dispose();
    }

    public async Task ResetAsync()
    {
        var opened = Connection.State is ConnectionState.Open;
        Context.ChangeTracker.Clear();

        if (opened is false)
            await Connection.OpenAsync();

        await _respawner.ResetAsync(Connection);

        if (opened is false)
            await Connection.CloseAsync();

        await SeedingHelper.SeedDatabaseAsync(Context);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(s =>
        {
            var descriptor = s.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));

            if (descriptor is not null)
                s.Remove(descriptor);

            s.AddDbContext<DatabaseContext>(x =>
                x.UseLazyLoadingProxies().UseNpgsql(Container.GetConnectionString()));

            s.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IAssemblyMarker>());
            s.AddFastEndpoints();
            s.AddValidation();
        });
    }

    private async Task InitializeContextAsync()
    {
        Context = Services.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
        await Context.Database.EnsureCreatedAsync();
    }

    private async Task CreateConnectionAsync()
    {
        Connection = new NpgsqlConnection(Container.GetConnectionString());
        await Connection.TryOpenAsync(default);
    }

    private async Task InitializeRespawnerAsync()
    {
        _respawner = await Respawner.CreateAsync(Connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" }
        });
    }
}