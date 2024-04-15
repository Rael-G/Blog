namespace Blog.Application;


public interface ITagService : IBaseService<TagDto>
{
    /// <summary>
    /// Retrieves a page of posts associated with a specific tag, according to the tag's ID and the specified page number.
    /// </summary>
    /// <param name="id">The ID of the tag.</param>
    /// <param name="page">The page number.</param>
    /// <returns>A tag DTO representing the tag along with its associated posts on the specified page, or null if the tag does not exist.</returns>
    Task<TagDto?> GetTagPage(Guid id, int page);

    /// <summary>
    /// Calculates the total number of pages for posts associated with a specific tag based on the total number of posts and the page size.
    /// </summary>
    /// <param name="id">The ID of the tag.</param>
    /// <returns>The total number of pages.</returns>
    Task<int> GetPageCount(Guid id);
}