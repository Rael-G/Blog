using Blog.Domain;

namespace Blog.Application;

public interface ICommentService
{
    void Create(Comment comment);
    void Update(Comment comment);
    void Delete(Comment comment);
    Task<CommentDto?> Get(Guid id);
    Task<IEnumerable<CommentDto>> GetAll();
}
