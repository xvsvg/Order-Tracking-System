using Application.Handlers.Extensions;
using Application.Validation.Extensions;
using Application.Validation.Middleware;
using Infrastructure.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Presentation.Endpoints.Extensions;

namespace Playground.Web;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDatabaseContext(o => o
            .UseNpgsql("Host=localhost;Port=1433;Database=ots;Username=postgres;Password=postgres")
            .UseLazyLoadingProxies());
    
        builder.Services.AddHandlers(builder.Configuration);
        builder.Services.AddEndpoints(builder.Configuration);
        builder.Services.AddValidation();
        builder.Services.AddTransient<ValidationMappingMiddleware>();

        var app = builder.Build();

        app.UseMiddleware<ValidationMappingMiddleware>();
        app.UseEndpoints();
        app.Run();
    }
}