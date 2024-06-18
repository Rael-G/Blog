using AutoMapper;
using Blog.Domain;
using Microsoft.AspNetCore.Identity;


namespace Blog.Application;
public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;

    public AuthService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Token?> LoginAsync(UserDto userDto)
    {
        var user = await _repository.GetByUserName(userDto.UserName);

        if (user is null || string.IsNullOrWhiteSpace(userDto.PasswordHash))
            return null;

        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher
            .VerifyHashedPassword(user, user.PasswordHash, userDto.PasswordHash);

        if (result != PasswordVerificationResult.Success)
            return null;

        var token = TokenService.GenerateToken(user);

        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpiryTime = token.Creation.AddDays(TokenService.DaysToExpiry);
        _repository.Update(user);
        await _repository.Commit();
        return token;
    }

    public async Task<Token?> RegenarateTokenAsync(Token tokenInput)
    {
        var principal = TokenService.GetPrincipalFromToken(tokenInput.AccessToken);

        var user = await _repository.Get(TokenService.GetUserIdFromClaims(principal));

        if (user == null ||
            user.RefreshToken != tokenInput.RefreshToken ||
            user.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            return null;
        }

        var token = TokenService.GenerateToken(principal.Claims);

        user.RefreshTokenExpiryTime = token.Creation.AddDays(TokenService.DaysToExpiry);

        _repository.Update(user);
        await _repository.Commit();

        return token;
    }
}
