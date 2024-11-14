namespace Blog.Domain;

public interface ICommentRepository : IBaseRepository<Comment>
{
    /// <summary>
    /// Retrieves all comments associated with a specific post.
    /// </summary>
    /// <param name="postId">The identifier of the post.</param>
    /// <returns>A task that represents the asynchronous operation, containing a collection of comments associated with the specified post.</returns>
    Task<IEnumerable<Comment>> GetAllByPostId(Guid postId);
}
