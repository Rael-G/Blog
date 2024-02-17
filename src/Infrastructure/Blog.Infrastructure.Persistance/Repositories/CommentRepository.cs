using Blog.Domain;
using Blog.Domain.Entities;

namespace Blog.Infrastructure.Persistance;

public class CommentRepository(ApplicationDbContext context) 
    : BaseRepository<Comment>(context), ICommentRepository
{

}

