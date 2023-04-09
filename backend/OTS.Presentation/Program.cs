using Microsoft.EntityFrameworkCore;
using OTS.Application;
using OTS.Persistence;
using OTS.Persistence.Configuration;

var builder = WebApplication.CreateBuilder(args);

var configuration = new PostgresConfiguration();
builder.Configuration.GetSection("PostgresConfiguration").Bind(configuration);

builder.Services.AddPersistence(o =>
    o.UseNpgsql(configuration.ToConnectionString("OTSDatabase"))
        .UseLazyLoadingProxies());

builder.Services.AddApplication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o =>
{
    o.AddPolicy("Loyal policy", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
