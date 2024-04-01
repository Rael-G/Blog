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
        .FirstOrDefaultAsync(t => t.Id == id);

    public override async Task<IEnumerable<Post>> GetAll()
        => await Context.Posts
        .AsNoTracking()
        .ToListAsync();
}
