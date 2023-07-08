using Application.Contracts.Tools;
using Infrastructure.DataAccess.Configuration;

namespace Presentation.WebAPI.Configuration;

public class WebApiConfiguration
{
    public WebApiConfiguration(IConfiguration configuration)
    {
        var postgresConfiguration = configuration
            .GetSection("PostgresConfiguration")
            .Get<PostgresConfiguration>();

        PostgresConfiguration = postgresConfiguration
                                ?? throw new ArgumentException(nameof(PostgresConfiguration));

        var paginationConfiguration = configuration
            .GetSection("Pagination")
            .Get<PaginationConfiguration>();

        PaginationConfiguration = paginationConfiguration
                                  ?? throw new ArgumentException(nameof(PaginationConfiguration));
    }

    public PostgresConfiguration PostgresConfiguration { get; }
    public PaginationConfiguration PaginationConfiguration { get; }
}