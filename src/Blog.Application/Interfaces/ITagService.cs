namespace Blog.Application;


public interface ITagService : IBaseService<TagDto>
{
    /// <summary>
    /// Retrieves all tags related to a post by its id.
    /// </summary>
    /// <param name="postId">The id of the post</param>
    /// <returns>A collection of tags</returns>
    Task<IEnumerable<TagDto>> GetAll(Guid postId);
}