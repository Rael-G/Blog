namespace Blog.Domain;

public interface ITagRepository : IBaseRepository<Tag>
{
    /// <summary>
    /// Retrieves a tag by its name.
    /// </summary>
    /// <param name="name">The name of the tag to retrieve.</param>
    /// <returns>The retrieved tag, or null if not found.</returns>
    Task<Tag?> GetByName(string name);

    /// <summary>
    /// Gets a page of posts associated with a specific tag with their associated tags, ordered by creation date of the posts, according to the page number and specified quantity.
    /// </summary>
    /// <param name="id">The ID of the tag.</param>
    /// <param name="page">The page number.</param>
    /// <param name="quantity">The quantity of posts per page.</param>
    /// <returns>A collection of posts associated with the specified tag on the specified page.</returns>
    Task<IEnumerable<Post>> GetTagPage(Guid id, int page, int quantity);

    /// <summary>
    /// Gets the total count of posts associated with a specific tag.
    /// </summary>
    /// <param name="id">The ID of the tag.</param>
    /// <returns>The total number of posts associated with the specified tag.</returns>
    Task<int> GetPostCount(Guid id);
}
