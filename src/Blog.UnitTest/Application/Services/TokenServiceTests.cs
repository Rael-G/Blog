using System.Security.Claims;
using Blog.Application;
using Blog.Domain;
using Microsoft.IdentityModel.Tokens;

namespace Blog.UnitTest.Application.Services;

public class TokenServiceTests
{
    private readonly User _user;
    public TokenServiceTests()
    {
        if (string.IsNullOrWhiteSpace(TokenService.SecretKey))
            TokenService.SecretKey = Guid.NewGuid().ToString();

        _user = new User(Guid.NewGuid(), "testUser", "password", ["role1", "role2"]);
    }

    [Fact]
    public void GenerateToken_ValidUser_ReturnsToken()
    {
        var result = TokenService.GenerateToken(_user);

        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
        Assert.True(result.Creation != default);
        Assert.True(result.Expiration != default);
    }

    [Fact]
    public void GenerateToken_Claims_ReturnsToken()
    {
        var claims = new List<Claim>
        {
            new("UserId", _user.Id.ToString()),
            new(ClaimTypes.Name, _user.UserName),
        };

        var result = TokenService.GenerateToken(claims);

        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
        Assert.True(result.Creation != default);
        Assert.True(result.Expiration != default);
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_ValidToken_ReturnsClaimsPrincipal()
    {
        var validToken = TokenService.GenerateToken(_user);

        var result = TokenService.GetPrincipalFromToken(validToken.AccessToken);

        Assert.NotNull(result);
        Assert.IsType<ClaimsPrincipal>(result);
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_InvalidToken_ThrowsSecurityTokenException()
    {
        var invalidToken = "invalid_token";

        Assert.Throws<SecurityTokenMalformedException>(() => TokenService.GetPrincipalFromToken(invalidToken));
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_ValidTokenFromInvalidKey_ThrowsSecurityTokenException()
    {
        var invalidToken = TokenService.GenerateToken(_user);
        var previousToken = TokenService.SecretKey;
        TokenService.SecretKey = Guid.NewGuid().ToString();

        Assert.Throws<SecurityTokenSignatureKeyNotFoundException>(() => TokenService.GetPrincipalFromToken(invalidToken.AccessToken));

        TokenService.SecretKey = previousToken;
    }
}
