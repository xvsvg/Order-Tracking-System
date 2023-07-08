using Presentation.WebAPI.Configuration;
using Presentation.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

var webApiConfiguration = new WebApiConfiguration(builder.Configuration);

builder.Services.ConfigureServices(builder.Configuration, webApiConfiguration);

var app = builder.Build().ConfigureApplication();

await app.RunAsync();