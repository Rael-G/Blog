namespace Blog.Domain;

public interface IPostRepository : IBaseRepository<Post>
{
    Task UpdatePostTag(Post post);
}
