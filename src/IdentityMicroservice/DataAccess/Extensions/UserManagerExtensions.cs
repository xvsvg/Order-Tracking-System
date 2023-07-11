using Application.Common.Exceptions;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Extensions;

public static class UserManagerExtensions
{
    public static async Task<OtsIdentityUser> GetByNameAsync(
        this UserManager<OtsIdentityUser> manager,
        string username,
        CancellationToken cancellationToken)
    {
        var user = await manager.FindByNameAsync(username);

        if (user is null)
            throw EntityNotFoundException.For<OtsIdentityUser>(username);

        return user;
    }
}