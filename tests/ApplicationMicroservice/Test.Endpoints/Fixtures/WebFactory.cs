using FastEndpoints;
using Infrastructure.DataAccess.DatabaseContexts;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Test.Tools.Helpers;
using Testcontainers.PostgreSql;
using Xunit;
using Program = Playground.Web.Program;

namespace Test.Endpoints.Fixtures;

public class WebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string User = "postgres";
    private const string Password = "postgres";
    private const string Database = "postgres";

    public WebFactory()
    {
        Container = new PostgreSqlBuilder()
            .WithUsername(User)
            .WithPassword(Password)
            .WithDatabase(Database)
            .Build();
    }

    public PostgreSqlContainer Container { get; }
    public DatabaseContext Context { get; private set; } = null!;

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

            s.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Application.Handlers.IAssemblyMarker>());
            s.AddFastEndpoints();
        });
    }

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
        Context = Services.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
        await Context.Database.EnsureCreatedAsync();
        await SeedingHelper.SeedDatabaseAsync(Context);
    }

    public async Task ResetAsync()
    {
        Context.ChangeTracker.Clear();
        await SeedingHelper.SeedDatabaseAsync(Context);
    }

    public new async Task DisposeAsync()
    {
        await Context.DisposeAsync();
        await Container.DisposeAsync();
    }
}