using Blog.Domain;

namespace Blog.Application;

public interface ICommentService
{
    /// <summary>
    /// Creates a new comment.
    /// </summary>
    /// <param name="commentDto">The comment to create.</param>
    void Create(CommentDto commentDto);

    /// <summary>
    /// Updates an existing comment.
    /// </summary>
    /// <param name="commentDto">The comment to update.</param>
    void Update(CommentDto commentDto);

    /// <summary>
    /// Deletes an existing comment.
    /// </summary>
    /// <param name="commentDto">The comment to delete.</param>
    void Delete(CommentDto commentDto);


    /// <summary>
    /// Retrieves a comment by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the comment to retrieve.</param>
    /// <returns>The retrieved comment, or null if not found.</returns>
    Task<CommentDto?> Get(Guid id);

    /// <summary>
    /// Retrieves all comments.
    /// </summary>
    /// <returns>A collection of comments.</returns>
    Task<IEnumerable<CommentDto>> GetAll();
}
