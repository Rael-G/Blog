﻿using Blog.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.Application;

public static class TokenService
{
    /// <summary>
    /// Gets or sets the secret key used for token generation and validation.
    /// </summary>
    public static string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Specifies the default expiration time for access tokens in minutes.
    /// </summary>
    public const int MinutesToExpiry = 30;

    /// <summary>
    /// Specifies the default expiration time for refresh tokens in days.
    /// </summary>
    public const int DaysToExpiry = 7;

    /// <summary>
    /// Generates an authentication token for the specified user and user roles.
    /// </summary>
    /// <param name="user">The user for whom the token is generated.</param>
    /// <param name="userRoles">The roles associated with the user.</param>
    /// <returns>The generated authentication token.</returns>
    public static Token GenerateToken(User user)
    {
        var accessToken = GenerateAccessToken(user);

        return GenerateToken(accessToken);
    }

    /// <summary>
    /// Regen an authentication token based on the provided claims and refresh token.
    /// </summary>
    /// <param name="claims">The claims to include in the token.</param>
    /// <param name="refreshToken">The refreshToken to include in the token</param>
    /// <returns>The generated authentication token.</returns>
    public static Token RegenToken(IEnumerable<Claim> claims, string refreshToken)
    {
        var accessToken = GenerateAccessToken(claims);

        return GenerateToken(accessToken, refreshToken);
    }

    /// <summary>
    /// Retrieves a ClaimsPrincipal from a JWT token.
    /// </summary>
    /// <param name="token">The JWT token.</param>
    /// <returns>The ClaimsPrincipal extracted from the token.</returns>
    public static ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var key = Encoding.ASCII.GetBytes(SecretKey);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = false
        };
        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals
            (SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
            throw new SecurityTokenException("Invalid Token");

        return principal;
    }

    /// <summary>
    /// Retrieves the user ID from the claims principal.
    /// </summary>
    /// <param name="user">The claims principal containing the user's claims.</param>
    /// <returns>The user ID extracted from the claims.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the user or the user ID claim is null.</exception>
    /// <exception cref="FormatException">Thrown when the user ID claim cannot be parsed as a GUID.</exception>
    public static Guid GetUserIdFromClaims(ClaimsPrincipal user)
    {
        var userId = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value ??
            throw new AppException("ClaimsPrincipal is Invalid");
        return Guid.Parse(userId);
    }

    private static Token GenerateToken(string accessToken, string? refreshToken = null)
    {
        var now = DateTime.UtcNow;
        return new Token
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken?? GenerateRefreshToken(),
            Creation = now,
            Expiration = now.AddMinutes(MinutesToExpiry)
        };
    }

    private static string GenerateAccessToken(User user)
    {
        var claimsIdentity = GenerateClaimsIdentity(user);

        return GenerateAccessToken(claimsIdentity);
    }

    private static string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var claimsIdentity = GenerateClaimsIdentity(claims);

        return GenerateAccessToken(claimsIdentity);
    }

    // Generates a JWT token based on the provided claims identity.
    private static string GenerateAccessToken(ClaimsIdentity claimsIdentity)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),

            Expires = DateTime.UtcNow.AddMinutes(MinutesToExpiry),

            Subject = claimsIdentity
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    // Generates the claims for the JWT token, including user-specific and role claims.
    private static ClaimsIdentity GenerateClaimsIdentity(User user)
    {
        var claimsIdentity = new ClaimsIdentity(
            [
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            ]);

        foreach (var role in user.Roles)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        return claimsIdentity;
    }

    private static ClaimsIdentity GenerateClaimsIdentity(IEnumerable<Claim> claims)
    {
        var claimsIdentity = new ClaimsIdentity(claims);

        return claimsIdentity;
    }
}