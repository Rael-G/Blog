using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistance;

public class PostRepository(ApplicationDbContext context)
    : BaseRepository<Post>(context), IPostRepository
{
    public async Task<Post?> GetByTitle(string title)
        => await Context.Posts.Where(p => p.Title == title)
            .FirstOrDefaultAsync();

    public override async Task<Post?> Get(Guid id)
        => await Context.Posts
            .AsNoTracking()
            .Include(p => p.Comments)
            .Include(p => p.User)
            .Include(p => p.Tags)
                .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task UpdatePostTag(Post post) {
        var existentPostTags = await Context.PostTag.Where(pt => pt.PostId == post.Id).ToListAsync();
        var postTagsToDelete = existentPostTags.Except(post.Tags);
        var postTagsToCreate = post.Tags.Except(existentPostTags);
        Context.PostTag.RemoveRange(postTagsToDelete);
        Context.PostTag.AddRange(postTagsToCreate);
    }

    public async Task<IEnumerable<Post>> GetPage(int page, int quantity)
        => await Context.Posts
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.Tags)
                .ThenInclude(pt => pt.Tag)
            .OrderByDescending(p => p.CreatedTime)
            .Skip((page - 1) * quantity)
            .Take(quantity)
            .ToListAsync();

    public async Task<int> GetCount()
        => await Context.Posts.CountAsync();
}