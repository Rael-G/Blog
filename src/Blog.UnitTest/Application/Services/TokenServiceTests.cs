using System.Security.Claims;
using Blog.Application;
using Blog.Domain;
using FluentAssertions;
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

     [Fact]
    public void GetUserIdFromClaims_ValidClaims_ReturnsUserId()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim("UserId", _user.Id.ToString()),
            new Claim(ClaimTypes.Name, _user.UserName),
            // Adicionar outras claims se necessário para outros testes
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        // Act
        Action act = () => TokenService.GetUserIdFromClaims(claimsPrincipal);

        // Assert
        act.Should().NotThrow(); // Verifica se não ocorre exceção
        Guid userId = TokenService.GetUserIdFromClaims(claimsPrincipal);
        userId.Should().Be(_user.Id); // Verifica se o ID retornado corresponde ao ID do usuário
    }

    [Fact]
    public void GetUserIdFromClaims_ClaimsWithoutUserId_ThrowsAppException()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _user.UserName),
            // Adicionar outras claims se necessário para outros testes
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        // Act
        Action act = () => TokenService.GetUserIdFromClaims(claimsPrincipal);

        // Assert
        act.Should().Throw<AppException>().WithMessage("ClaimsPrincipal is Invalid");
    }
}
