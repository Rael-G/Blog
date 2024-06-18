using Blog.Application;
using Blog.WebApi;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blog.UnitTest.WebApi.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _authController;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _authController = new AuthController(_mockAuthService.Object);
    }

    [Fact]
    public async Task LogIn_InvalidModelState_ReturnsBadRequest()
    {
        _authController.ModelState.AddModelError("UserName", "Required");
        var loginInput = new LoginInputModel { UserName = "", Password = "password" };

        var result = await _authController.LogIn(loginInput);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task LogIn_InvalidCredentials_ReturnsUnauthorized()
    {
        var loginInput = new LoginInputModel { UserName = "invalidUser", Password = "invalidPassword" };
        var userDto = loginInput.InputToDto();

        _mockAuthService.Setup(service => service.LoginAsync(It.IsAny<UserDto>())).ReturnsAsync(() => null);

        var result = await _authController.LogIn(loginInput);

        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult!.Value.Should().BeEquivalentTo(new { Message = "Wrong Username or Password." });
    }

    [Fact]
    public async Task LogIn_ValidCredentials_ReturnsOkWithToken()
    {
        var loginInput = new LoginInputModel { UserName = "validUser", Password = "validPassword" };
        var expectedToken = new Token
        {
            AccessToken = "accessToken",
            RefreshToken = "refreshToken",
            Creation = DateTime.UtcNow,
            Expiration = DateTime.UtcNow.AddMinutes(TokenService.MinutesToExpiry)
        };

        _mockAuthService.Setup(service => service.LoginAsync(It.IsAny<UserDto>())).ReturnsAsync(expectedToken);

        var result = await _authController.LogIn(loginInput);

        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedToken);
    }

     [Fact]
    public async Task RegenerateToken_InvalidModelState_ReturnsBadRequest()
    {
        _authController.ModelState.AddModelError("AccessToken", "Required");
        var tokenInput = new TokenInputModel { AccessToken = "", RefreshToken = "refreshToken" };

        var result = await _authController.RegenerateToken(tokenInput);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task RegenerateToken_InvalidToken_ReturnsUnauthorized()
    {
        var tokenInput = new TokenInputModel { AccessToken = "invalidAccessToken", RefreshToken = "invalidRefreshToken" };
        _mockAuthService.Setup(service => service.RegenarateTokenAsync(It.IsAny<Token>())).ReturnsAsync(() => null);

        var result = await _authController.RegenerateToken(tokenInput);

        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult!.Value.Should().BeEquivalentTo(new { Message = "Invalid Token" });
    }

    [Fact]
    public async Task RegenerateToken_ValidToken_ReturnsOkWithToken()
    {
        var tokenInput = new TokenInputModel { AccessToken = "validAccessToken", RefreshToken = "validRefreshToken" };
        var expectedToken = new Token
        {
            AccessToken = "newAccessToken",
            RefreshToken = "newRefreshToken",
            Creation = DateTime.UtcNow,
            Expiration = DateTime.UtcNow.AddMinutes(TokenService.MinutesToExpiry)
        };

        _mockAuthService.Setup(service => service.RegenarateTokenAsync(It.IsAny<Token>())).ReturnsAsync(expectedToken);

        var result = await _authController.RegenerateToken(tokenInput);

        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedToken);
    }
}
