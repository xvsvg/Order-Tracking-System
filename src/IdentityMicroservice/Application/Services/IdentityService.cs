using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

    public async Task<TokenDto> LoginAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
            throw new UnauthorizedException("Wrong login or password");

        var areEqual = await _userManager.CheckPasswordAsync(user, password);

        if (areEqual is false)
            throw new UnauthorizedException("Wrong login or password");

        var roles = await _userManager.GetRolesAsync(user);
        var claims = roles
            .Select(role => new Claim(ClaimTypes.Role, role))
            .Append(new Claim(ClaimTypes.Name, user.UserName ?? string.Empty))
            .Append(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        var token = CreateToken(claims);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.Add(_configuration.RefreshTokenExpiration);

        await _userManager.UpdateAsync(user);

        return new TokenDto(
            AccessToken: new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken: refreshToken,
            Expiration: token.ValidTo);
    }

    public async Task RegisterAsync(Guid Id, string username, string password,
        CancellationToken cancellationToken)
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
    }

    public async Task<TokenDto> RefreshToken(string accessToken, string refreshToken)
    {
        var principal = GetPrincipalFromExpiredToken(accessToken);
        var user = await _userManager.GetByNameAsync(principal.Identity?.Name ?? string.Empty, default);

        var newAccessToken = CreateToken(principal.Claims.ToList());
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new TokenDto(
            AccessToken: new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken: newRefreshToken,
            Expiration: newAccessToken.ValidTo);
    }

    private JwtSecurityToken CreateToken(IEnumerable<Claim> claims)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            expires: DateTime.UtcNow.Add(_configuration.TokenExpiration),
            claims: claims,
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.Sha256));

        return token;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidation = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret)),
            ValidateLifetime = true
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(token, tokenValidation, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken)
            throw new SecurityTokenValidationException("Invalid token");

        return principal;
    }
}