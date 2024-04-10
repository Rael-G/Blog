namespace Blog.Domain;

public interface ITagRepository : IBaseRepository<Tag>
{
    /// <summary>
    /// Retrieves a tag by its name.
    /// </summary>
    /// <param name="name">The name of the tag to retrieve.</param>
    /// <returns>The retrieved tag, or null if not found.</returns>
    Task<Tag?> GetByName(string name);
}
