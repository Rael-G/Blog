using System.Security.Claims;
using Blog.Application;
using Blog.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Blog.UnitTest.Application.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly IAuthService _authService;
    private readonly PasswordHasher<User> _passwordHasher = new();

    private readonly UserDto _userDto;
    private readonly User _user;
    private readonly Token _tokenInput;

    public AuthServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _authService = new AuthService(_mockRepository.Object);
        if (string.IsNullOrWhiteSpace(TokenService.SecretKey))
            TokenService.SecretKey = Guid.NewGuid().ToString();

       _userDto = new(){Id = Guid.NewGuid(), UserName = "nonexistentuser", PasswordHash = "password" };
       _user = new (_userDto.Id, _userDto.UserName, _userDto.PasswordHash);
       _user.PasswordHash = _passwordHasher.HashPassword(_user, _user.PasswordHash);

        _tokenInput = TokenService.GenerateToken(_user);
    }
    
    [Fact]
    public async Task LoginAsync_UserNotFound_ReturnsNull()
    {
        _mockRepository.Setup(repo => repo.GetByUserName(_userDto.UserName)).ReturnsAsync(() => null);

        var result = await _authService.LoginAsync(_userDto);

        result.Should().BeNull();
        _mockRepository.Verify(repo => repo.GetByUserName(_userDto.UserName), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_PasswordIsNullOrWhiteSpace_ReturnsNull()
    {
        _userDto.PasswordHash = "";
        var result = await _authService.LoginAsync(_userDto);

        result.Should().BeNull();
        _mockRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
        _mockRepository.Verify(repo => repo.Commit(), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ReturnsNull()
    {
        _userDto.PasswordHash = "invalidPassword";
        
        _mockRepository.Setup(repo => repo.GetByUserName(_userDto.UserName)).ReturnsAsync(_user);

        var result = await _authService.LoginAsync(_userDto);

        result.Should().BeNull();
        _mockRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
        _mockRepository.Verify(repo => repo.Commit(), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsToken()
    {

        _mockRepository.Setup(repo => repo.GetByUserName(_userDto.UserName)).ReturnsAsync(_user);

        var result = await _authService.LoginAsync(_userDto);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Token>();
        _user.RefreshToken.Should().Be(result!.RefreshToken);
        _user.RefreshTokenExpiryTime.Should().Be(result.Creation.AddDays(TokenService.DaysToExpiry));
        _mockRepository.Verify(repo => repo.Update(_user), Times.Once);
        _mockRepository.Verify(repo => repo.Commit(), Times.Once);
    }

    [Fact]
    public async Task RegenarateTokenAsync_UserNotFound_ReturnsNull()
    {
        _mockRepository.Setup(repo => repo.Get(_userDto.Id)).ReturnsAsync(() => null);

        var result = await _authService.RegenarateTokenAsync(_tokenInput);

        result.Should().BeNull();
        _mockRepository.Verify(repo => repo.Get(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task RegenarateTokenAsync_InvalidRefreshToken_ReturnsNull()
    {
        var user = new User(Guid.NewGuid(), "username", "hashedPassword") { RefreshToken = "validRefreshToken", RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(5) };
        _mockRepository.Setup(repo => repo.Get(_userDto.Id)).ReturnsAsync(user);

        var result = await _authService.RegenarateTokenAsync(_tokenInput);

        result.Should().BeNull();
        _mockRepository.Verify(repo => repo.Get(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task RegenarateTokenAsync_ExpiredRefreshToken_ReturnsNull()
    {
        var user = new User(Guid.NewGuid(), "username", "hashedPassword") { RefreshToken = _tokenInput.RefreshToken, RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(-5) };
        _mockRepository.Setup(repo => repo.Get(_userDto.Id)).ReturnsAsync(user);

        var result = await _authService.RegenarateTokenAsync(_tokenInput);

        result.Should().BeNull();
        _mockRepository.Verify(repo => repo.Get(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task RegenarateTokenAsync_ValidToken_ReturnsNewToken()
    {
        var user = new User(Guid.NewGuid(), "username", "hashedPassword") { RefreshToken = _tokenInput.RefreshToken, RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(5) };
        _mockRepository.Setup(repo => repo.Get(_userDto.Id)).ReturnsAsync(user);

        var result = await _authService.RegenarateTokenAsync(_tokenInput);

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Token>();
        user.RefreshTokenExpiryTime.Should().Be(result!.Creation.AddDays(TokenService.DaysToExpiry));
        _mockRepository.Verify(repo => repo.Update(user), Times.Once);
        _mockRepository.Verify(repo => repo.Commit(), Times.Once);
    }
}
