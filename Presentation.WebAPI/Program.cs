using Presentation.WebAPI.Configuration;
using Presentation.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

var webApiConfiguration = new WebApiConfiguration(builder.Configuration, builder.Environment);
builder.Configuration.AddSecrets(webApiConfiguration);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build().ConfigureApplication();

await app.RunAsync();