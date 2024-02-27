using Blog.Domain;

namespace Blog.Application;

public class PostService : IPostService
{
    public void Create(Post post)
    {
        throw new NotImplementedException();
    }

    public void Update(Post post)
    {
        throw new NotImplementedException();
    }

    public void Delete(Post post)
    {
        throw new NotImplementedException();
    }

    public Task<PostDto?> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PostDto>> GetAll()
    {
        throw new NotImplementedException();
    }
}
