using Blog.Domain;

namespace Blog.Application;

public interface ICommentService
{
    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="comment">The comment to create.</param>
    void Create(Comment comment);

    /// <summary>
    /// Updates an existing comment.
    /// </summary>
    /// <param name="comment">The comment to update.</param>
    void Update(Comment comment);

    /// <summary>
    /// Deletes an existing comment.
    /// </summary>
    /// <param name="comment">The comment to delete.</param>
    void Delete(Comment comment);


    /// <summary>
    /// Retrieves a post by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the post to retrieve.</param>
    /// <returns>The retrieved post, or null if not found.</returns>
    Task<CommentDto?> Get(Guid id);

    /// <summary>
    /// Retrieves all posts.
    /// </summary>
    /// <returns>A collection of posts.</returns>
    Task<IEnumerable<CommentDto>> GetAll();
}
