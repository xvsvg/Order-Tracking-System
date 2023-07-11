using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Exceptions;
using Application.Dto;
using Application.Mapping;
using Application.Tools;
using DataAccess.Extensions;
using DataAccess.Models;
using DataAccess.Tools;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Application.Services;

public class IdentityService
{
    private readonly RoleManager<OtsIdentityRole> _roleManager;
    private readonly UserManager<OtsIdentityUser> _userManager;
    private readonly TokenConfiguration _configuration;

    public IdentityService(
        RoleManager<OtsIdentityRole> roleManager,
        UserManager<OtsIdentityUser> userManager,
        TokenConfiguration configuration)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task AuthorizeAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
            throw new UnauthorizedException("Wrong credentials");
        
        var areEqual = await _userManager.CheckPasswordAsync(user, password);

        if (areEqual is false)
            throw new UnauthorizedException("Wrong password");
    }

    public async Task<UserDto> RegisterAsync(Guid Id, string username, string password, CancellationToken cancellationToken)
    {
        var user = new OtsIdentityUser
        {
            Id = Id,
            UserName = username,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user, password);

        result.EnsureSucceeded();

        await _userManager.AddToRoleAsync(user, IdentityRoleNames.DefaultUserRoleName);

        return user.ToDto();
    }

    public async Task<string> GetUserTokenAsync(string username, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetByNameAsync(username, cancellationToken);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = roles
            .Select(role => new Claim(ClaimTypes.Role, role))
            .Append(new Claim(ClaimTypes.Name, username))
            .Append(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()))
            .Append(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));

        var token = new JwtSecurityToken(
            _configuration.Issuer,
            _configuration.Audience,
            claims,
            expires: DateTime.UtcNow.Add(_configuration.Expiration),
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.Sha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}