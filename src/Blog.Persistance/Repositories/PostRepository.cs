using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistance;

public class PostRepository(ApplicationDbContext context)
    : BaseRepository<Post>(context), IPostRepository
{
    public override async Task<Post?> Get(Guid id)
        => await Context.Posts
        .AsNoTracking()
        .Include(p => p.Comments)
        .Include(p => p.Tags)
        .ThenInclude(pt => pt.Tag)
        .FirstOrDefaultAsync(p => p.Id == id);
}