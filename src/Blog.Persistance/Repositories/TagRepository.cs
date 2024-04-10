using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistance;

public class TagRepository(ApplicationDbContext context)
    : BaseRepository<Tag>(context), ITagRepository
{
    public override async Task<Tag?> Get(Guid id)
        => await Context.Tags
        .AsNoTracking()
        .Include(t => t.Posts)
        .ThenInclude(pt => pt.Post)
        .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<Tag?> GetByName(string name)
        => await Context.Tags
        .AsNoTracking()
        .Include(t => t.Posts)
        .ThenInclude(pt => pt.Post)
        .FirstOrDefaultAsync(t => t.Name == name);

    public async Task<IEnumerable<Tag>> GetAll(Guid postId)
    {
        var tagIds = await Context.PostTag
        .Where(pt => pt.PostId == postId)
        .Select(pt => pt.TagId)
        .ToListAsync();

        return await Context.Tags
        .Where(t => tagIds.Contains(t.Id))
        .ToListAsync();
    }

}