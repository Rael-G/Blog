namespace Blog.Application;


public interface IAuthService
{
    Task<Token?> LoginAsync(UserDto userDto);

    Task<Token?> RegenarateTokenAsync(Token token);
}

