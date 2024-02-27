using Blog.Domain;

namespace Blog.Infrastructure.Persistance;

public class CommentRepository(ApplicationDbContext context) 
    : BaseRepository<Comment>(context), ICommentRepository
{

}

