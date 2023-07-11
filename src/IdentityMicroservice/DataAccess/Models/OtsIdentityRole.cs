using Microsoft.AspNetCore.Identity;

namespace DataAccess.Models;

public class OtsIdentityRole : IdentityRole<Guid>
{
    protected OtsIdentityRole()
    {
    }

    public OtsIdentityRole(string roleName)
        : base(roleName)
    {
    }
}