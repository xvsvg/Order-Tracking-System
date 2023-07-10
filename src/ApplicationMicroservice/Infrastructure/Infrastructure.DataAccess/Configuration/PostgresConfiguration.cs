namespace Infrastructure.DataAccess.Configuration;

public class PostgresConfiguration
{
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;

    public string ToConnectionString(string databaseName)
    {
        return $"Host={Host};Port={Port};Database={databaseName};Username={Username};Password={Password};";
    }
}