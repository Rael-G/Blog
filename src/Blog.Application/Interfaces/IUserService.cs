using Blog.Domain;

namespace Blog.Application;

public interface IUserService : IBaseService<UserDto>
{
    public Task<UserDto> GetByUserName(string username);

    public Task UpdatePassword(UserDto userDto);

    public Task UpdateRoles(UserDto userDto);

    /// <summary>
    /// Retrieves a page of posts associated with a specific user, according to the user's ID and the specified page number.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="page">The page number.</param>
    /// <returns>A user DTO representing the user along with its associated posts on the specified page, or null if the user does not exist.</returns>
    Task<UserDto?> GetUserPage(Guid id, int page);

    /// <summary>
    /// Calculates the total number of pages for posts associated with a specific user based on the total number of posts and the page size.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The total number of pages.</returns>
    Task<int> GetPageCount(Guid id);
}
