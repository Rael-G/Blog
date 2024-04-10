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
    /// Retrieves all tags related to a post by its id
    /// </summary>
    /// <param name="postId">The id of the post</param>
    /// <returns>A collection of tags</returns>
    Task<IEnumerable<Tag>> GetAll(Guid postId);
}
