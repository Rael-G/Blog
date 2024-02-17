using Blog.Domain;

namespace Blog.Application;

public class CommentService : ICommentService
{
    public void Create(Comment comment)
    {
        throw new NotImplementedException();
    }

    public void Update(Comment comment)
    {
        throw new NotImplementedException();
    }

    public void Delete(Comment comment)
    {
        throw new NotImplementedException();
    }

    public Task<CommentDto?> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CommentDto>> GetAll()
    {
        throw new NotImplementedException();
    }
}
