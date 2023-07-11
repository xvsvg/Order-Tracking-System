namespace DataAccess.Tools;

public class TokenConfiguration
{
    public string Secret { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public TimeSpan Expiration { get; set; } = default!;
}