using Blog.Domain;

namespace Blog.Application;

public interface IPostService
{
    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="postDto">The post to create.</param>
    void Create(PostDto postDto);

    /// <summary>
    /// Updates an existing post.
    /// </summary>
    /// <param name="postDto">The post to update.</param>
    void Update(PostDto postDto);

    /// <summary>
    /// Deletes an existing post.
    /// </summary>
    /// <param name="postDto">The post to delete.</param>
    void Delete(PostDto postDto);

    /// <summary>
    /// Retrieves a post by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the post to retrieve.</param>
    /// <returns>The retrieved post, or null if not found.</returns>
    Task<PostDto?> Get(Guid id);

    /// <summary>
    /// Retrieves all posts.
    /// </summary>
    /// <returns>A collection of posts.</returns>
    Task<IEnumerable<PostDto>> GetAll();
}
