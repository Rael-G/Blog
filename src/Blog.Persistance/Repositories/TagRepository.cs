using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistance;

public class TagRepository(ApplicationDbContext context)
    : BaseRepository<Tag>(context), ITagRepository
{
    public override async Task<Tag?> Get(Guid id)
        => await Context.Tags
        .AsNoTracking()
        .Include(p => p.Posts)
        .FirstOrDefaultAsync(t => t.Id == id);
}