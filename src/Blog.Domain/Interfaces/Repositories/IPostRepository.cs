namespace Blog.Domain;

public interface IPostRepository : IBaseRepository<Post>
{
    Task UpdatePostTag(Post post);
    Task<IEnumerable<Post>> GetPage(int page, int quantity);
    Task<int> GetCount();
}
