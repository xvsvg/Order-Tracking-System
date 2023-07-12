using Microsoft.AspNetCore.Identity;

namespace DataAccess.Models;

public class OtsIdentityUser : IdentityUser<Guid>
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}