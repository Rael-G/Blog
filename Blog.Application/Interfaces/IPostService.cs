namespace Blog.Application;

public interface IPostService : IBaseService<PostDto>
{
    /// <summary>
    /// Updates an existing post.
    /// </summary>
    /// <param name="postDto">The post to update.</param>
    new Task Update(PostDto postDto);

    /// <summary>
    /// Retrieves a page of posts, according to the specified page number.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <returns>A collection of post DTOs.</returns>
    Task<IEnumerable<PostDto>> GetPage(int page);

    /// <summary>
    /// Calculates the total number of pages based on the total number of posts and the page size.
    /// </summary>
    /// <returns>The total number of pages.</returns>
    Task<int> GetPageCount();
}
