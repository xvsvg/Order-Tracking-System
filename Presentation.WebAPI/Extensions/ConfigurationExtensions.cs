using Presentation.WebAPI.Configuration;
using VaultSharp.Extensions.Configuration;

namespace Presentation.WebAPI.Extensions;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddSecrets(this IConfigurationBuilder builder,
        WebApiConfiguration configuration)
    {
        var uri = configuration.ASPNETCORE_ENVIRONMENT is "Development"
            ? "http://localhost:8200"
            : "http://vault:8200";

        var cgf = builder
            .AddVaultConfiguration(() => new VaultOptions(
                uri,
                configuration.VaultToken),
            $"{typeof(ConfigurationExtensions).Assembly.GetName().Name}/{configuration.ASPNETCORE_ENVIRONMENT}",
            "ots");

        return builder;
    }
}