namespace Blog.Application;


public interface IAuthService
{
    Task<Token?> LoginAsync(UserDto userDto, string password);

    Task<Token?> RegenarateTokenAsync(string accessToken, string refreshToken);
}

