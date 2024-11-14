using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistance;

public class CommentRepository(ApplicationDbContext context)
    : BaseRepository<Comment>(context), ICommentRepository
{
    public async Task<IEnumerable<Comment>> GetAllByPostId(Guid postId)
        => await Context.Comments
        .AsNoTracking()
        .Where(c => c.PostId == postId)
        .ToListAsync();
}

