namespace Blog.Application;

public interface IPostService : IBaseService<PostDto>
{
    /// <summary>
    /// Updates an existing post.
    /// </summary>
    /// <param name="postDto">The post to update.</param>
    new Task Update(PostDto postDto);

    /// <summary>
    /// Retrieves all tags related to a post by its id.
    /// </summary>
    /// <param name="postId">The id of the post</param>
    /// <returns>A collection of tags</returns>
    Task<IEnumerable<TagDto>?> GetTags(Guid postId);
}
