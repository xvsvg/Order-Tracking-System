using Microsoft.AspNetCore.Identity;

namespace DataAccess.Extensions;

public static class IdentityResultExtensions
{
    public static void EnsureSucceeded(this IdentityResult result)
    {
        if (result.Succeeded is false)
            throw new ApplicationException(
                string.Join(' ',
                    result.Errors.Select(x => x.Description)));
    }
}