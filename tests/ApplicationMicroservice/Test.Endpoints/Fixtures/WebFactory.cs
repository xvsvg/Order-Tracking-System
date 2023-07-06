using Application.Handlers;
using Application.Validation.Extensions;
using FastEndpoints;
using Infrastructure.DataAccess.DatabaseContexts;
using Infrastructure.Seeding.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Playground.Web;
using Testcontainers.PostgreSql;
using Xunit;

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

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
        Context = Services.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
        await Context.Database.EnsureCreatedAsync();
        await SeedingHelper.SeedDatabaseAsync(Context);
    }

    public new async Task DisposeAsync()
    {
        await Context.DisposeAsync();
        await Container.DisposeAsync();
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

    public async Task ResetAsync()
    {
        Context.ChangeTracker.Clear();
        await SeedingHelper.SeedDatabaseAsync(Context);
    }
}