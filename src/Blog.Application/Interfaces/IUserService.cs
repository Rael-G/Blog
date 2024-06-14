using Blog.Domain;

namespace Blog.Application;

/// <summary>
/// Interface defining operations related to user management in the application layer.
/// </summary>
public interface IUserService : IBaseService<UserDto>
{
    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>A user DTO representing the user.</returns>
    Task<UserDto> GetByUserName(string username);

    /// <summary>
    /// Updates the password of a user.
    /// </summary>
    /// <param name="userDto">The user DTO containing updated password information.</param>
    Task UpdatePassword(UserDto userDto);

    /// <summary>
    /// Updates the roles of a user.
    /// </summary>
    /// <param name="userDto">The user DTO containing updated role information.</param>
    Task UpdateRoles(UserDto userDto);

    /// <summary>
    /// Retrieves a page of posts associated with a specific user.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="page">The page number.</param>
    /// <returns>
    /// A user DTO representing the user along with its associated posts on the specified page,
    /// or null if the user does not exist.
    /// </returns>
    Task<UserDto?> GetUserPage(Guid id, int page);

    /// <summary>
    /// Calculates the total number of pages for posts associated with a specific user.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The total number of pages.</returns>
    Task<int> GetPageCount(Guid id);
}
