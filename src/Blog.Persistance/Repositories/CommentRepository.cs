using Blog.Domain;

namespace Blog.Persistance;

public class CommentRepository(ApplicationDbContext context) 
    : BaseRepository<Comment>(context), ICommentRepository
{

}

