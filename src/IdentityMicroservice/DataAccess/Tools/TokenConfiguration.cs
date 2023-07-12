namespace DataAccess.Tools;

public class TokenConfiguration
{
    public string Secret { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public TimeSpan TokenExpiration { get; set; } = default!;
    public TimeSpan RefreshTokenExpiration { get; set; } = default!;
}