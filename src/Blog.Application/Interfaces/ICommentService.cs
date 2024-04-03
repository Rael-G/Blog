namespace Blog.Application;

public interface ICommentService : IBaseService<CommentDto>
{
    /// <summary>
    /// Retrieves all <see cref="CommentDto"> associated with a specific post.
    /// </summary>
    /// <param name="postId">The identifier of the post.</param>
    /// <returns>A collection of <see cref="CommentDto"> associated with the specified post.</returns>
    Task<IEnumerable<CommentDto>> GetAll(Guid postId);
}