using Application.Contracts.Tools;

namespace Presentation.WebAPI.Configuration;

public class WebApiConfiguration
{
    public WebApiConfiguration(IConfiguration configuration, IWebHostEnvironment environment)
    {
        var paginationConfiguration = configuration
            .GetSection("Pagination")
            .Get<PaginationConfiguration>();

        PaginationConfiguration = paginationConfiguration
                                  ?? throw new ArgumentException(nameof(PaginationConfiguration));

        var vaultToken = configuration
            .GetValue<string>("Vault:Token");

        VaultToken = vaultToken ?? string.Empty;

        ASPNETCORE_ENVIRONMENT = environment.EnvironmentName;
    }

    public PaginationConfiguration PaginationConfiguration { get; }
    public string VaultToken { get; }
    public string ASPNETCORE_ENVIRONMENT { get; }
}