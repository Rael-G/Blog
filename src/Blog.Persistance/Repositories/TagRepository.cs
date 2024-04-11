using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistance;

public class TagRepository(ApplicationDbContext context)
    : BaseRepository<Tag>(context), ITagRepository
{
    public override async Task<Tag?> Get(Guid id)
        => await Context.Tags
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<Tag?> GetByName(string name)
        => await Context.Tags
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.Name == name);

    public async Task<IEnumerable<Post>> GetTagPage(Guid id, int page, int quantity)
        => await Context.PostTag
            .AsNoTracking()
            .Where(pt => pt.TagId == id)
            .Include(pt => pt.Post!)
                .ThenInclude(p => p.Tags)
                    .ThenInclude(t => t.Tag)
            .OrderByDescending(p => p.Post!.CreatedTime)
            .Skip((page - 1) * quantity)
            .Take(quantity)
            .Select(pt => pt.Post!)
            .ToListAsync();

    public async Task<int> GetPostCount(Guid id)
        => await Context.PostTag
            .AsNoTracking()
            .Where(pt => pt.TagId == id)
            .CountAsync();
}