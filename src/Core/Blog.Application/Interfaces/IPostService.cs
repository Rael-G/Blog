using Blog.Domain;

namespace Blog.Application;

public interface IPostService
{
    void Create(Post post);
    void Update(Post post);
    void Delete(Post post);
    Task<PostDto?> Get(Guid id);
    Task<IEnumerable<PostDto>> GetAll();
}
