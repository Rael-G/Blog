namespace Blog.Domain;

public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    /// Retrieves a user based on the specified username.
    /// </summary>
    /// <param name="userName">The username of the user to retrieve.</param>
    /// <returns>
    /// The user entity that matches the specified username, or null if not found.
    /// </returns>
    public Task<User?> GetByUserName(string userName);

    /// <summary>
    /// Gets a page of posts associated with a specific user with their associated tags, ordered by creation date of the posts, according to the page number and specified quantity.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="page">The page number.</param>
    /// <param name="quantity">The quantity of posts per page.</param>
    /// <returns>A collection of posts associated with the specified user on the specified page.</returns>
    Task<IEnumerable<Post>> GetPostPage(Guid id, int page, int quantity);

    /// <summary>
    /// Gets the total count of posts associated with a specific user.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The total number of posts associated with the specified tag.</returns>
    Task<int> GetPostCount(Guid id);
}
