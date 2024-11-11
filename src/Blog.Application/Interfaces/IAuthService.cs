namespace Blog.Application;

public interface IAuthService
{
    /// <summary>
    /// Asynchronously performs user login and generates a token if authentication is successful.
    /// </summary>
    /// <param name="userDto">The data transfer object (DTO) containing user credentials.</param>
    /// <returns>A token representing the user's authentication session, or null if login fails.</returns>
    Task<Token?> LoginAsync(UserDto userDto);

    /// <summary>
    /// Asynchronously regenerates a new token based on the provided token.
    /// </summary>
    /// <param name="tokenInput">The token input containing the access token and refresh token.</param>
    /// <returns>A new token if regeneration is successful, or null if regeneration fails.</returns>
    Task<Token?> RegenarateTokenAsync(Token token);
}
