using AutoMapper;
using Blog.Domain;
using Microsoft.AspNetCore.Identity;


namespace Blog.Application;
public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public AuthService(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Token?> LoginAsync(UserDto userDto, string password)
    {
        var user = await _repository.GetByUserName(userDto.UserName);

        if (user is null)
            return null;

        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher
            .VerifyHashedPassword(user, user.PasswordHash, password);

        if (result != PasswordVerificationResult.Success)
            return null;

        var token = TokenService.GenerateToken(user);

        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpiryTime = token.Creation.AddDays(TokenService.DaysToExpiry);
        _repository.Update(user);
        await _repository.Commit();
        return token;
    }

    public async Task<Token?> RegenarateTokenAsync(string accessToken, string refreshToken)
    {
        var principal = TokenService.GetPrincipalFromToken(accessToken);

        var user = await _repository.GetByUserName(principal.Identity?.Name!);

        if (user == null ||
            user.RefreshToken != refreshToken ||
            user.RefreshTokenExpiryTime < DateTime.UtcNow)
            return null;

        var token = TokenService.GenerateToken(principal.Claims);

        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpiryTime = token.Creation.AddDays(TokenService.DaysToExpiry);

        _repository.Update(user);
        await _repository.Commit();

        return token;
    }
}
