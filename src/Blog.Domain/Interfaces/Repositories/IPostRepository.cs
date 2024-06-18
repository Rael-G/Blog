namespace Blog.Domain;

public interface IPostRepository : IBaseRepository<Post>
{
    /// <summary>
    /// Retrieves a post based on the specified title.
    /// </summary>
    /// <param name="title">The title of the post to retrieve.</param>
    /// <returns>
    /// The post entity that matches the specified title, or null if no post is found.
    /// </returns>
    Task<Post?> GetByTitle(string title);

    /// <summary>
    /// Updates the tags of a post.
    /// </summary>
    /// <param name="post">The post to be updated.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdatePostTag(Post post);

    /// <summary>
    /// Gets a page of posts ordered by creation date, with their associated tags, according to the page number and specified quantity.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="quantity">The quantity of posts per page.</param>
    /// <returns>A collection of posts on the specified page.</returns>
    Task<IEnumerable<Post>> GetPage(int page, int quantity);
    
    /// <summary>
    /// Gets the total count of posts.
    /// </summary>
    /// <returns>The total number of posts.</returns>
    Task<int> GetCount();
}
